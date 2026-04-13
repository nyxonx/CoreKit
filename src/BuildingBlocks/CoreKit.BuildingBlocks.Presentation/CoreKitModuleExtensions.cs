using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreKit.BuildingBlocks.Presentation;

public static class CoreKitModuleExtensions
{
    public static IServiceCollection AddCoreKitModules(
        this IServiceCollection services,
        IConfiguration configuration,
        params ICoreKitModule[] modules)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(modules);

        var registeredModules = modules.ToArray();

        services.AddSingleton<IReadOnlyList<ICoreKitModule>>(registeredModules);
        services.TryAddSingleton<CoreKitModuleInitializationPipeline>();

        foreach (var module in registeredModules)
        {
            module.AddServices(services, configuration);
        }

        return services;
    }

    public static IEndpointRouteBuilder MapRegisteredCoreKitModules(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var modules = endpoints.ServiceProvider.GetRequiredService<IReadOnlyList<ICoreKitModule>>();

        foreach (var module in modules)
        {
            module.MapEndpoints(endpoints);
        }

        return endpoints;
    }

    public static async Task InitializeRegisteredCoreKitModulesAsync(
        this IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var pipeline = services.GetRequiredService<CoreKitModuleInitializationPipeline>();
        await pipeline.InitializeAsync(services, configuration, cancellationToken);
    }
}
