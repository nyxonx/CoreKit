using System.Text.Json;
using CoreKit.AppHost.Contracts.Rpc;
using CoreKit.AppHost.Server.Rpc;
using CoreKit.BuildingBlocks.Application;
using CoreKit.Modules.Customers.Application;
using CoreKit.Modules.Customers.Infrastructure;
using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreKit.Modules.Customers.Tests;

public sealed class CustomersModuleTests
{
    [Fact]
    public async Task CreateCustomerCommandValidator_ReturnsError_WhenNameIsMissing()
    {
        var validator = new CreateCustomerCommandValidator();

        var errors = await validator.ValidateAsync(new CreateCustomerCommand(string.Empty, "test@example.com"));

        Assert.Single(errors);
        Assert.Equal("validation_error", errors[0].Code);
    }

    [Fact]
    public async Task CustomerService_CreatesAndReadsCustomers_FromTenantDatabase()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "customers-service-test");
            await using var scope = provider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<ICustomerService>();

            await service.CreateCustomerAsync(new CreateCustomerRequest("Contoso", "hello@contoso.test"));
            var customers = await service.GetCustomersAsync();

            Assert.Single(customers);
            Assert.Equal("Contoso", customers[0].Name);
            Assert.Equal("hello@contoso.test", customers[0].Email);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task RpcDispatcher_CreatesAndListsCustomers()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "customers-rpc-test");
            await using var scope = provider.CreateAsyncScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<RpcDispatcher>();

            var createResponse = await dispatcher.DispatchAsync(
                CreateRequest("customers.create", """{"name":"Northwind","email":"sales@northwind.test"}"""));

            var listResponse = await dispatcher.DispatchAsync(
                CreateRequest("customers.list", "{}"));

            Assert.True(createResponse.Succeeded);
            Assert.True(listResponse.Succeeded);

            var createdCustomer = Assert.IsType<CoreKit.Modules.Customers.Application.CustomerDto>(createResponse.Data);
            var customers =
                Assert.IsAssignableFrom<IReadOnlyList<CoreKit.Modules.Customers.Application.CustomerDto>>(
                    listResponse.Data);

            Assert.Equal("Northwind", createdCustomer.Name);
            Assert.Single(customers);
            Assert.Equal("sales@northwind.test", customers[0].Email);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    private static ServiceProvider CreateServices(string tempRoot, string tenantIdentifier)
    {
        var databasePath = Path.Combine(tempRoot, $"{tenantIdentifier}.db");
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddCoreKitApplication(typeof(CustomersApplicationAssemblyMarker).Assembly);
        services.AddSingleton(new RpcOperationRegistry(typeof(CustomersApplicationAssemblyMarker).Assembly));
        services.AddScoped<RpcDispatcher>();
        services.AddScoped<ITenantContextAccessor>(_ =>
            new TenantContextAccessor
            {
                TenantContext = new TenantContext
                {
                    Identifier = tenantIdentifier,
                    Name = tenantIdentifier,
                    Host = $"{tenantIdentifier}.local",
                    ConnectionString = $"Data Source={databasePath}"
                }
            });
        services.AddScoped<ITenantConnectionStringProvider, TenantConnectionStringProvider>();
        services.AddCustomersInfrastructure();
        services.AddScoped<ICustomerService, CustomerService>();

        return services.BuildServiceProvider();
    }

    private static RpcRequest CreateRequest(string operation, string payloadJson)
    {
        using var document = JsonDocument.Parse(payloadJson);
        return new RpcRequest(operation, document.RootElement.Clone());
    }

    private static string CreateTempRoot()
    {
        var tempRoot = Path.Combine(Path.GetTempPath(), "corekit-customers-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);
        return tempRoot;
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
