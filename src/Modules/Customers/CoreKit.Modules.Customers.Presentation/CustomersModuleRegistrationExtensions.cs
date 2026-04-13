using CoreKit.Modules.Customers.Application;
using CoreKit.Modules.Customers.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Customers.Presentation;

public static class CustomersModuleRegistrationExtensions
{
    public static IServiceCollection AddCustomersModule(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCustomersInfrastructure();
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }

    public static IEndpointRouteBuilder MapCustomersModule(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/api/modules/customers").WithTags("Customers");

        group.MapGet(
            "/status",
            () => Results.Ok(
                new
                {
                    module = "Customers",
                    status = "registered",
                    transport = "rpc"
                }));

        return endpoints;
    }
}
