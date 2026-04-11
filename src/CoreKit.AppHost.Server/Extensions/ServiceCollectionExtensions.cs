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
        services.AddCoreKitModules();

        return services;
    }
}
