using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantCatalogSeedService(TenantCatalogDbContext tenantCatalogDbContext)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        _ = await tenantCatalogDbContext.Tenants
            .AsNoTracking()
            .AnyAsync(cancellationToken);
    }
}
