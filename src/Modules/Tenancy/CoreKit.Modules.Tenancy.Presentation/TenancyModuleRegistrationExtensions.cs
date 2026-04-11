using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Tenancy.Presentation;

public static class TenancyModuleRegistrationExtensions
{
    public static IServiceCollection AddTenancyModule(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services;
    }

    public static IEndpointRouteBuilder MapTenancyModule(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/api/modules/tenancy").WithTags("Tenancy");

        group.MapGet(
            "/status",
            () => Results.Ok(
                new
                {
                    module = "Tenancy",
                    status = "registered"
                }));

        return endpoints;
    }
}
