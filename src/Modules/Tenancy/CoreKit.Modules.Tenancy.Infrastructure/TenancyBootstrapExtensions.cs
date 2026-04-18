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

        var catalogMigrationRunner = scope.ServiceProvider.GetRequiredService<TenantCatalogMigrationRunner>();
        await catalogMigrationRunner.MigrateAsync(cancellationToken);

        var catalogSeedService = scope.ServiceProvider.GetRequiredService<TenantCatalogSeedService>();
        await catalogSeedService.SeedAsync(cancellationToken);

        var validator = scope.ServiceProvider.GetRequiredService<TenantConfigurationValidator>();
        await validator.ValidateAsync(cancellationToken);

        var provisioningService = scope.ServiceProvider.GetRequiredService<TenantProvisioningService>();
        await provisioningService.ProvisionAllAsync(cancellationToken);
    }
}
