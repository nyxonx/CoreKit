using CoreKit.Modules.Tenancy.Application;
using CoreKit.Modules.Tenancy.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantAdministrationService(
    TenantCatalogDbContext tenantCatalogDbContext,
    TenantProvisioningService tenantProvisioningService,
    IConfiguration configuration) : ITenantAdministrationService
{
    public async Task<IReadOnlyList<TenantCatalogDto>> GetTenantsAsync(CancellationToken cancellationToken = default)
    {
        return await tenantCatalogDbContext.Tenants
            .AsNoTracking()
            .OrderBy(tenant => tenant.Identifier)
            .Select(tenant => new TenantCatalogDto(
                tenant.Identifier,
                tenant.Name,
                tenant.Host,
                tenant.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<TenantCatalogDto> CreateTenantAsync(
        CreateTenantRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var identifier = request.Identifier.Trim().ToLowerInvariant();
        var host = request.Host.Trim().ToLowerInvariant();

        var identifierExists = await tenantCatalogDbContext.Tenants.AnyAsync(
            tenant => tenant.Identifier == identifier,
            cancellationToken);

        if (identifierExists)
        {
            throw new InvalidOperationException($"Tenant '{identifier}' already exists.");
        }

        var hostExists = await tenantCatalogDbContext.Tenants.AnyAsync(
            tenant => tenant.Host == host,
            cancellationToken);

        if (hostExists)
        {
            throw new InvalidOperationException($"Host '{host}' is already assigned to another tenant.");
        }

        var tenant = new TenantCatalogEntry
        {
            Identifier = identifier,
            Name = request.Name.Trim(),
            Host = host,
            IsActive = true,
            ConnectionString = BuildTenantConnectionString(identifier)
        };

        tenantCatalogDbContext.Tenants.Add(tenant);
        await tenantCatalogDbContext.SaveChangesAsync(cancellationToken);
        await tenantProvisioningService.ProvisionTenantAsync(tenant, cancellationToken);

        return new TenantCatalogDto(
            tenant.Identifier,
            tenant.Name,
            tenant.Host,
            tenant.IsActive);
    }

    public async Task<TenantCatalogDto?> SetTenantActivationAsync(
        string tenantIdentifier,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantIdentifier);

        var normalizedTenantIdentifier = tenantIdentifier.Trim().ToLowerInvariant();
        var tenant = await tenantCatalogDbContext.Tenants.SingleOrDefaultAsync(
            entry => entry.Identifier == normalizedTenantIdentifier,
            cancellationToken);

        if (tenant is null)
        {
            return null;
        }

        tenant.IsActive = isActive;
        await tenantCatalogDbContext.SaveChangesAsync(cancellationToken);

        return new TenantCatalogDto(
            tenant.Identifier,
            tenant.Name,
            tenant.Host,
            tenant.IsActive);
    }

    private string BuildTenantConnectionString(string identifier)
    {
        return LocalSqliteConnectionStringResolver.ResolveFileName(
            configuration,
            $"corekit.{identifier}.tenant.db");
    }
}
