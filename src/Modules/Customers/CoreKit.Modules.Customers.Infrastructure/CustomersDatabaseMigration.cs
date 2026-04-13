using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Customers.Infrastructure;

public sealed class CustomersDatabaseMigration : ITenantDatabaseMigration
{
    public string Name => "2026-04-13-customers";

    public async Task ApplyAsync(
        TenantProvisioningContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        var options = new DbContextOptionsBuilder<CustomersDbContext>()
            .UseSqlite(context.Tenant.ConnectionString)
            .Options;

        await using var dbContext = new CustomersDbContext(options);
        await dbContext.EnsureSchemaAsync(cancellationToken);
    }
}
