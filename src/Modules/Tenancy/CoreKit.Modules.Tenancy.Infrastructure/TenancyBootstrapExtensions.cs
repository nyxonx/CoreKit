using CoreKit.Modules.Tenancy.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public static class TenancyBootstrapExtensions
{
    public static async Task InitializeTenancyAsync(
        this IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        await using var scope = services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TenantCatalogDbContext>();

        await dbContext.Database.MigrateAsync(cancellationToken);

        var identifier = configuration["Tenancy:Seed:Identifier"] ?? "bootstrap";
        var name = configuration["Tenancy:Seed:Name"] ?? "Bootstrap Tenant";
        var host = configuration["Tenancy:Seed:Host"] ?? "localhost";
        var connectionString =
            configuration.GetConnectionString("DefaultTenantDatabase")
            ?? "Data Source=corekit.bootstrap.tenant.db";

        var existingTenant = await dbContext.Tenants.SingleOrDefaultAsync(
            tenant => tenant.Identifier == identifier,
            cancellationToken);

        if (existingTenant is null)
        {
            dbContext.Tenants.Add(
                new TenantCatalogEntry
                {
                    Identifier = identifier,
                    Name = name,
                    Host = host,
                    IsActive = true,
                    ConnectionString = connectionString
                });
        }
        else
        {
            existingTenant.Name = name;
            existingTenant.Host = host;
            existingTenant.IsActive = true;
            existingTenant.ConnectionString = connectionString;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var validator = scope.ServiceProvider.GetRequiredService<TenantConfigurationValidator>();
        await validator.ValidateAsync(cancellationToken);
    }
}
