using CoreKit.AppHost.Server.Diagnostics;
using CoreKit.AppHost.Server.Rpc;
using CoreKit.BuildingBlocks.Application;
using CoreKit.BuildingBlocks.Presentation;

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

        services.AddHttpContextAccessor();
        services.AddHealthChecks()
            .AddCheck<TenantCatalogHealthCheck>("tenant-catalog-db", tags: ["ready"])
            .AddCheck<RpcOperationsHealthCheck>("rpc-operations", tags: ["ready"]);
        services.AddCoreKitApplication(applicationAssemblies);
        services.AddSingleton(new RpcOperationRegistry(applicationAssemblies));
        services.AddScoped<IAuditEventWriter, LoggingAuditEventWriter>();
        services.AddScoped<RpcDispatcher>();
        services.AddCoreKitModules(configuration);

        return services;
    }
}
