using CoreKit.Modules.Tenancy.Domain;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantDatabaseBootstrapper(
    ITenantContextAccessor tenantContextAccessor,
    TenantProvisioningService tenantProvisioningService)
{
    public Task EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        var tenantContext = tenantContextAccessor.TenantContext
            ?? throw new InvalidOperationException("Tenant context is not available for database provisioning.");

        return tenantProvisioningService.ProvisionTenantAsync(
            new TenantCatalogEntry
            {
                Identifier = tenantContext.Identifier,
                Name = tenantContext.Name,
                Host = tenantContext.Host,
                IsActive = true,
                ConnectionString = tenantContext.ConnectionString
            },
            cancellationToken);
    }
}
