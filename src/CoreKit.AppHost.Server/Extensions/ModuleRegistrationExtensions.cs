using CoreKit.BuildingBlocks.Presentation;

namespace CoreKit.AppHost.Server.Extensions;

public static class ModuleRegistrationExtensions
{
    public static IServiceCollection AddCoreKitModules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        return services.AddCoreKitModules(configuration, [.. CoreKitModuleCatalog.All]);
    }

    public static IEndpointRouteBuilder MapCoreKitModules(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        return endpoints.MapRegisteredCoreKitModules();
    }

    public static Task InitializeCoreKitModulesAsync(
        this IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        return services.InitializeRegisteredCoreKitModulesAsync(configuration, cancellationToken);
    }
}
