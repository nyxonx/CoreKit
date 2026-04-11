using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantCatalogDbContextFactory : IDesignTimeDbContextFactory<TenantCatalogDbContext>
{
    public TenantCatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TenantCatalogDbContext>();
        optionsBuilder.UseSqlite("Data Source=corekit.catalog.db");

        return new TenantCatalogDbContext(optionsBuilder.Options);
    }
}
