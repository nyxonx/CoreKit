using CoreKit.Modules.Tenancy.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantResolutionService(
    TenantCatalogDbContext dbContext,
    IMemoryCache memoryCache,
    IOptions<TenantCatalogCacheOptions> cacheOptions)
{
    public async Task<TenantResolutionResult> ResolveAsync(
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var headerIdentifier = httpContext.Request.Headers["X-Tenant"].FirstOrDefault()?.Trim();

        if (!string.IsNullOrWhiteSpace(headerIdentifier))
        {
            var tenantByIdentifier = await GetByIdentifierAsync(headerIdentifier, cancellationToken);

            return CreateResult(tenantByIdentifier, $"Unknown tenant '{headerIdentifier}'.");
        }

        var host = httpContext.Request.Host.Host;

        if (string.IsNullOrWhiteSpace(host))
        {
            return TenantResolutionResult.Failure("Tenant could not be resolved because the request host was missing.");
        }

        var tenantByHost = await GetByHostAsync(host, cancellationToken);

        return CreateResult(tenantByHost, $"Unknown tenant host '{host}'.");
    }

    private Task<TenantCatalogEntry?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        var cacheKey = $"tenant-catalog:identifier:{identifier}";
        return GetOrCreateAsync(
            cacheKey,
            () => dbContext.Tenants
                .AsNoTracking()
                .SingleOrDefaultAsync(tenant => tenant.Identifier == identifier, cancellationToken));
    }

    private Task<TenantCatalogEntry?> GetByHostAsync(string host, CancellationToken cancellationToken)
    {
        var cacheKey = $"tenant-catalog:host:{host}";
        return GetOrCreateAsync(
            cacheKey,
            () => dbContext.Tenants
                .AsNoTracking()
                .SingleOrDefaultAsync(tenant => tenant.Host == host, cancellationToken));
    }

    private async Task<TenantCatalogEntry?> GetOrCreateAsync(
        string cacheKey,
        Func<Task<TenantCatalogEntry?>> factory)
    {
        if (memoryCache.TryGetValue(cacheKey, out TenantCatalogEntry? cachedTenant))
        {
            return cachedTenant;
        }

        var tenant = await factory();

        if (tenant is null)
        {
            return null;
        }

        var cachedCopy = Clone(tenant);

        memoryCache.Set(
            cacheKey,
            cachedCopy,
            TimeSpan.FromSeconds(cacheOptions.Value.TtlSeconds));

        return cachedCopy;
    }

    private static TenantCatalogEntry Clone(TenantCatalogEntry tenant)
    {
        return new TenantCatalogEntry
        {
            Id = tenant.Id,
            Identifier = tenant.Identifier,
            Name = tenant.Name,
            Host = tenant.Host,
            IsActive = tenant.IsActive,
            ConnectionString = tenant.ConnectionString
        };
    }

    private static TenantResolutionResult CreateResult(
        TenantCatalogEntry? tenant,
        string missingTenantMessage)
    {
        if (tenant is null)
        {
            return TenantResolutionResult.Failure(missingTenantMessage);
        }

        if (!tenant.IsActive)
        {
            return TenantResolutionResult.Failure($"Tenant '{tenant.Identifier}' is inactive.");
        }

        return TenantResolutionResult.Success(tenant);
    }
}
