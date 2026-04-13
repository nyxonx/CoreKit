using System.Reflection;
using CoreKit.BuildingBlocks.Presentation;
using CoreKit.Modules.Identity.Application;
using CoreKit.Modules.Identity.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Identity.Presentation;

public sealed class IdentityModule : ICoreKitModule
{
    public string Name => "Identity";

    public IReadOnlyCollection<Assembly> ApplicationAssemblies =>
        [typeof(IdentityApplicationAssemblyMarker).Assembly];

    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddIdentityInfrastructure(configuration);
        services.AddIdentityModule();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapIdentityModule();
    }

    public void ConfigurePipeline(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
    }

    public Task InitializeAsync(
        IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        return services.InitializeIdentityAsync(configuration, cancellationToken);
    }
}
