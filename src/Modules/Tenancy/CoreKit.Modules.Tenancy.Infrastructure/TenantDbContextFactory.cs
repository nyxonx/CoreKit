using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantDbContextFactory(ITenantConnectionStringProvider connectionStringProvider)
    : ITenantDbContextFactory
{
    public TenantAppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TenantAppDbContext>()
            .UseSqlite(connectionStringProvider.GetRequiredConnectionString())
            .Options;

        return new TenantAppDbContext(options);
    }
}
