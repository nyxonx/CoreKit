using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Customers.Infrastructure;

public static class CustomersInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddCustomersInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped(
            serviceProvider =>
            {
                var connectionStringProvider =
                    serviceProvider.GetRequiredService<ITenantConnectionStringProvider>();

                var options = new DbContextOptionsBuilder<CustomersDbContext>()
                    .UseSqlite(connectionStringProvider.GetRequiredConnectionString())
                    .Options;

                return new CustomersDbContext(options);
            });

        return services;
    }
}
