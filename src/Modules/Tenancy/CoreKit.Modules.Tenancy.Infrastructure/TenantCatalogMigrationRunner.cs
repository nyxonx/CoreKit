using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantCatalogMigrationRunner(TenantCatalogDbContext tenantCatalogDbContext)
{
    public Task MigrateAsync(CancellationToken cancellationToken = default) =>
        tenantCatalogDbContext.Database.MigrateAsync(cancellationToken);
}
