using CoreKit.Modules.Tenancy.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantProvisioningService(
    TenantCatalogDbContext tenantCatalogDbContext,
    TenantDatabaseMigrationRunner tenantDatabaseMigrationRunner,
    TenantSeedDataRunner tenantSeedDataRunner)
{
    public async Task ProvisionAllAsync(CancellationToken cancellationToken = default)
    {
        var activeTenants = await tenantCatalogDbContext.Tenants
            .AsNoTracking()
            .Where(tenant => tenant.IsActive)
            .OrderBy(tenant => tenant.Identifier)
            .ToListAsync(cancellationToken);

        foreach (var tenant in activeTenants)
        {
            await ProvisionTenantAsync(tenant, cancellationToken);
        }
    }

    public async Task ProvisionTenantAsync(
        TenantCatalogEntry tenant,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        var context = new TenantProvisioningContext(tenant);

        await tenantDatabaseMigrationRunner.MigrateAsync(context, cancellationToken);
        await tenantSeedDataRunner.SeedAsync(context, cancellationToken);
    }
}
