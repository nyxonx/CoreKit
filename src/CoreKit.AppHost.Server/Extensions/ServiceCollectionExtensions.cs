using CoreKit.Modules.Identity.Infrastructure;
using CoreKit.Modules.Tenancy.Infrastructure;

namespace CoreKit.AppHost.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreKitAppHost(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddHealthChecks();
        services.AddTenancyInfrastructure(configuration);
        services.AddIdentityInfrastructure(configuration);
        services.AddCoreKitModules();

        return services;
    }
}
