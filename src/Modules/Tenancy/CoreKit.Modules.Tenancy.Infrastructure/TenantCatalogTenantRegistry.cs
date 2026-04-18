using CoreKit.Modules.Tenancy.Application;
using CoreKit.Modules.Tenancy.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantCatalogTenantRegistry(
    TenantCatalogDbContext dbContext,
    IMemoryCache memoryCache,
    IOptions<TenantCatalogCacheOptions> cacheOptions) : ITenantRegistry
{
    public Task<TenantRuntimeInfo?> GetByIdentifierAsync(
        string identifier,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"tenant-registry:identifier:{identifier}";
        return GetOrCreateAsync(
            cacheKey,
            () => dbContext.Tenants
                .AsNoTracking()
                .Where(tenant => tenant.Identifier == identifier)
                .Select(ProjectToRuntimeInfo())
                .SingleOrDefaultAsync(cancellationToken));
    }

    public Task<TenantRuntimeInfo?> GetByHostAsync(
        string host,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"tenant-registry:host:{host}";
        return GetOrCreateAsync(
            cacheKey,
            () => dbContext.Tenants
                .AsNoTracking()
                .Where(tenant => tenant.Host == host)
                .Select(ProjectToRuntimeInfo())
                .SingleOrDefaultAsync(cancellationToken));
    }

    private async Task<TenantRuntimeInfo?> GetOrCreateAsync(
        string cacheKey,
        Func<Task<TenantRuntimeInfo?>> factory)
    {
        if (memoryCache.TryGetValue(cacheKey, out TenantRuntimeInfo? cachedTenant))
        {
            return cachedTenant;
        }

        var tenant = await factory();

        if (tenant is null)
        {
            return null;
        }

        memoryCache.Set(
            cacheKey,
            tenant,
            TimeSpan.FromSeconds(cacheOptions.Value.TtlSeconds));

        return tenant;
    }

    private static Expression<Func<TenantCatalogEntry, TenantRuntimeInfo>> ProjectToRuntimeInfo()
    {
        return tenant => new TenantRuntimeInfo(
            tenant.Identifier,
            tenant.Name,
            tenant.Host,
            tenant.IsActive,
            tenant.ConnectionString);
    }
}
