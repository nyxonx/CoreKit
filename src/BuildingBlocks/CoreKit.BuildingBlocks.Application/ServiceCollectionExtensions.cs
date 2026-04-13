using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.BuildingBlocks.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreKitApplication(this IServiceCollection services, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        var distinctAssemblies = assemblies.Distinct().ToArray();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(distinctAssemblies));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        foreach (var assembly in distinctAssemblies)
        {
            RegisterValidators(services, assembly);
        }

        return services;
    }

    private static void RegisterValidators(IServiceCollection services, Assembly assembly)
    {
        var validatorTypes = assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false })
            .Select(type => new
            {
                ImplementationType = type,
                ServiceTypes = type.GetInterfaces()
                    .Where(@interface =>
                        @interface.IsGenericType
                        && @interface.GetGenericTypeDefinition() == typeof(IValidator<>))
                    .ToArray()
            })
            .Where(candidate => candidate.ServiceTypes.Length > 0);

        foreach (var validatorType in validatorTypes)
        {
            foreach (var serviceType in validatorType.ServiceTypes)
            {
                services.AddScoped(serviceType, validatorType.ImplementationType);
            }
        }
    }
}
