using CoreKit.Modules.Identity.Presentation;
using CoreKit.Modules.Tenancy.Presentation;

namespace CoreKit.AppHost.Server.Extensions;

public static class ModuleRegistrationExtensions
{
    public static IServiceCollection AddCoreKitModules(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddIdentityModule();
        services.AddTenancyModule();

        return services;
    }

    public static IEndpointRouteBuilder MapCoreKitModules(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapIdentityModule();
        endpoints.MapTenancyModule();

        return endpoints;
    }
}
