using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CoreKit.BuildingBlocks.Presentation;
using CoreKit.Modules.Tenancy.Application;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public static class TenancyInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddTenancyInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var connectionString = LocalSqliteConnectionStringResolver.Resolve(
            configuration,
            "TenantCatalogDatabase",
            "corekit.catalog.db");

        services.AddMemoryCache();
        services.AddOptions<TenantCatalogCacheOptions>()
            .Bind(configuration.GetSection(TenantCatalogCacheOptions.SectionName))
            .Validate(
                options => options.TtlSeconds > 0,
                "Tenant catalog cache TTL must be greater than zero.");
        services.AddOptions<TenantCatalogMaintenanceJobOptions>()
            .Bind(configuration.GetSection(TenantCatalogMaintenanceJobOptions.SectionName))
            .Validate(
                options => options.IntervalMinutes > 0,
                "Tenant catalog maintenance job interval must be greater than zero.");
        services.AddOptions<TenantRegistryOptions>()
            .Bind(configuration.GetSection(TenantRegistryOptions.SectionName))
            .Validate(
                options => !options.IsRemoteMode() || !string.IsNullOrWhiteSpace(options.BaseUrl),
                "Tenant registry remote mode requires a BaseUrl.");
        services.AddOptions<ControlPlaneHostOptions>()
            .Configure(options =>
            {
                options.Hosts =
                    configuration.GetSection(ControlPlaneHostOptions.SectionName).Get<string[]>()
                    ?? [];
            });

        services.AddHttpClient(nameof(RemoteTenantRegistry));
        services.AddDbContext<TenantCatalogDbContext>(
            options => options.UseSqlite(connectionString));

        services.AddScoped<ITenantRegistry>(
            serviceProvider =>
            {
                var registryOptions = serviceProvider.GetRequiredService<IOptions<TenantRegistryOptions>>();

                return registryOptions.Value.IsRemoteMode()
                    ? serviceProvider.GetRequiredService<RemoteTenantRegistry>()
                    : serviceProvider.GetRequiredService<TenantCatalogTenantRegistry>();
            });
        services.AddScoped<TenantCatalogTenantRegistry>();
        services.AddScoped<RemoteTenantRegistry>();
        services.AddScoped<TenantResolutionService>();
        services.AddScoped<ITenantContextAccessor, TenantContextAccessor>();
        services.AddScoped<ITenantConnectionStringProvider, TenantConnectionStringProvider>();
        services.AddScoped<ITenantDbContextFactory, TenantDbContextFactory>();
        services.AddScoped(
            serviceProvider => serviceProvider.GetRequiredService<ITenantDbContextFactory>().CreateDbContext());
        services.AddScoped<TenantCatalogMigrationRunner>();
        services.AddScoped<TenantCatalogSeedService>();
        services.AddScoped<TenantDatabaseMigrationRunner>();
        services.AddScoped<TenantSeedDataRunner>();
        services.AddScoped<TenantProvisioningService>();
        services.AddScoped<ITenantAdministrationService, TenantAdministrationService>();
        services.AddScoped<TenantDatabaseBootstrapper>();
        services.AddScoped<TenantConfigurationValidator>();
        services.AddScoped<ITenantDatabaseMigration, TenantMetadataMigration>();
        services.AddScoped<ITenantDatabaseMigration, TenantNotesDatabaseMigration>();
        services.AddScoped<ITenantSeedDataContributor, TenantMetadataSeedContributor>();
        services.AddCoreKitBackgroundJob<TenantCatalogMaintenanceBackgroundJob>();

        return services;
    }
}
