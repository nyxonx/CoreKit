using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Identity.Presentation;

public static class IdentityModuleRegistrationExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services;
    }

    public static IEndpointRouteBuilder MapIdentityModule(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/api/modules/identity").WithTags("Identity");

        group.MapGet(
            "/status",
            () => Results.Ok(
                new
                {
                    module = "Identity",
                    status = "registered"
                }));

        return endpoints;
    }
}
