using CoreKit.AppHost.Server.Rpc;
using CoreKit.BuildingBlocks.Application;
using CoreKit.Modules.Identity.Infrastructure;
using CoreKit.Modules.Tenancy.Application;
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
        services.AddCoreKitApplication(typeof(TenancyApplicationAssemblyMarker).Assembly);
        services.AddSingleton(new RpcOperationRegistry(typeof(TenancyApplicationAssemblyMarker).Assembly));
        services.AddScoped<RpcDispatcher>();
        services.AddTenancyInfrastructure(configuration);
        services.AddIdentityInfrastructure(configuration);
        services.AddCoreKitModules();

        return services;
    }
}
