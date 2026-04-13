using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace CoreKit.BuildingBlocks.Presentation;

public static class BackgroundJobServiceCollectionExtensions
{
    public static IServiceCollection AddCoreKitBackgroundJob<TJob>(this IServiceCollection services)
        where TJob : class, ICoreKitBackgroundJob
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<ICoreKitBackgroundJob, TJob>();
        services.TryAddSingleton<CoreKitBackgroundJobRegistry>();
        services.TryAddSingleton<CoreKitBackgroundJobHostedService>();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IHostedService, CoreKitBackgroundJobHostedService>(
                serviceProvider => serviceProvider.GetRequiredService<CoreKitBackgroundJobHostedService>()));

        return services;
    }
}
