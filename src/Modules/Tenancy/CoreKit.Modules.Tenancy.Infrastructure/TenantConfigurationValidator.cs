using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantConfigurationValidator(TenantCatalogDbContext tenantCatalogDbContext)
{
    public async Task ValidateAsync(CancellationToken cancellationToken = default)
    {
        var tenants = await tenantCatalogDbContext.Tenants
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var tenant in tenants)
        {
            if (string.IsNullOrWhiteSpace(tenant.ConnectionString))
            {
                throw new InvalidOperationException(
                    $"Tenant '{tenant.Identifier}' is missing a tenant database connection string.");
            }
        }
    }
}
