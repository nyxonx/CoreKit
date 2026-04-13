using CoreKit.AppHost.Server.Rpc;
using CoreKit.BuildingBlocks.Application;

namespace CoreKit.AppHost.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreKitAppHost(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var applicationAssemblies = CoreKitModuleCatalog.All
            .SelectMany(module => module.ApplicationAssemblies)
            .Distinct()
            .ToArray();

        services.AddHealthChecks();
        services.AddCoreKitApplication(applicationAssemblies);
        services.AddSingleton(new RpcOperationRegistry(applicationAssemblies));
        services.AddScoped<RpcDispatcher>();
        services.AddCoreKitModules(configuration);

        return services;
    }
}
