using System.Reflection;
using CoreKit.BuildingBlocks.Presentation;
using CoreKit.Modules.Tenancy.Application;
using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Tenancy.Presentation;

public sealed class TenancyModule : ICoreKitModule
{
    public string Name => "Tenancy";

    public IReadOnlyCollection<Assembly> ApplicationAssemblies =>
        [typeof(TenancyApplicationAssemblyMarker).Assembly];

    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddTenancyInfrastructure(configuration);
        services.AddTenancyModule();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapTenancyModule();
    }

    public Task InitializeAsync(
        IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        return services.InitializeTenancyAsync(configuration, cancellationToken);
    }
}
