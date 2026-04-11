using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public static class TenancyInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddTenancyInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var connectionString =
            configuration.GetConnectionString("TenantCatalogDatabase")
            ?? "Data Source=corekit.catalog.db";

        services.AddDbContext<TenantCatalogDbContext>(
            options => options.UseSqlite(connectionString));

        services.AddScoped<TenantResolutionService>();
        services.AddScoped<ITenantContextAccessor, TenantContextAccessor>();
        services.AddScoped<ITenantConnectionStringProvider, TenantConnectionStringProvider>();
        services.AddScoped<ITenantDbContextFactory, TenantDbContextFactory>();
        services.AddScoped(
            serviceProvider => serviceProvider.GetRequiredService<ITenantDbContextFactory>().CreateDbContext());
        services.AddScoped<TenantDatabaseBootstrapper>();
        services.AddScoped<TenantConfigurationValidator>();

        return services;
    }
}
