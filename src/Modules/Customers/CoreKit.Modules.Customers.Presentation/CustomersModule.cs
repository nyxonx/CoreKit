using System.Reflection;
using CoreKit.BuildingBlocks.Presentation;
using CoreKit.Modules.Customers.Application;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Customers.Presentation;

public sealed class CustomersModule : ICoreKitModule
{
    public string Name => "Customers";

    public IReadOnlyCollection<Assembly> ApplicationAssemblies =>
        [typeof(CustomersApplicationAssemblyMarker).Assembly];

    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddCustomersModule();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapCustomersModule();
    }

    public Task InitializeAsync(
        IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
