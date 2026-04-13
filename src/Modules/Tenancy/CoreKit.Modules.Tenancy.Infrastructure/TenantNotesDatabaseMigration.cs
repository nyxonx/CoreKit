using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantNotesDatabaseMigration : ITenantDatabaseMigration
{
    public string Name => "2026-04-13-tenancy-notes";

    public async Task ApplyAsync(
        TenantProvisioningContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        var options = new DbContextOptionsBuilder<TenantAppDbContext>()
            .UseSqlite(context.Tenant.ConnectionString)
            .Options;

        await using var dbContext = new TenantAppDbContext(options);
        await dbContext.EnsureSchemaAsync(cancellationToken);
    }
}
