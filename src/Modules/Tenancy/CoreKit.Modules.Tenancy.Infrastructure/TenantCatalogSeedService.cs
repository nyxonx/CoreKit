using CoreKit.Modules.Tenancy.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantCatalogSeedService(TenantCatalogDbContext tenantCatalogDbContext)
{
    public async Task SeedAsync(
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var tenants = BuildSeedTenants(configuration);

        foreach (var seedTenant in tenants)
        {
            var existingTenant = await tenantCatalogDbContext.Tenants.SingleOrDefaultAsync(
                tenant => tenant.Identifier == seedTenant.Identifier,
                cancellationToken);

            if (existingTenant is null)
            {
                tenantCatalogDbContext.Tenants.Add(seedTenant);
                continue;
            }

            existingTenant.Name = seedTenant.Name;
            existingTenant.Host = seedTenant.Host;
            existingTenant.IsActive = seedTenant.IsActive;
            existingTenant.ConnectionString = seedTenant.ConnectionString;
        }

        await tenantCatalogDbContext.SaveChangesAsync(cancellationToken);
    }

    private static IReadOnlyList<TenantCatalogEntry> BuildSeedTenants(IConfiguration configuration)
    {
        return
        [
            new TenantCatalogEntry
            {
                Identifier = configuration["Tenancy:Seed:Identifier"] ?? "bootstrap",
                Name = configuration["Tenancy:Seed:Name"] ?? "Bootstrap Tenant",
                Host = configuration["Tenancy:Seed:Host"] ?? "localhost",
                IsActive = true,
                ConnectionString =
                    configuration.GetConnectionString("DefaultTenantDatabase")
                    ?? "Data Source=corekit.bootstrap.tenant.db"
            },
            new TenantCatalogEntry
            {
                Identifier = "contoso",
                Name = "Contoso",
                Host = "contoso.local",
                IsActive = true,
                ConnectionString =
                    configuration.GetConnectionString("ContosoTenantDatabase")
                    ?? "Data Source=corekit.contoso.tenant.db"
            },
            new TenantCatalogEntry
            {
                Identifier = "fabrikam",
                Name = "Fabrikam",
                Host = "fabrikam.local",
                IsActive = true,
                ConnectionString =
                    configuration.GetConnectionString("FabrikamTenantDatabase")
                    ?? "Data Source=corekit.fabrikam.tenant.db"
            }
        ];
    }
}
