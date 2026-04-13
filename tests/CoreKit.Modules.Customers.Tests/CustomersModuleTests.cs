using System.Text.Json;
using CoreKit.AppHost.Contracts.Rpc;
using CoreKit.AppHost.Server.Rpc;
using CoreKit.BuildingBlocks.Application;
using CoreKit.BuildingBlocks.Presentation;
using CoreKit.Modules.Customers.Application;
using CoreKit.Modules.Customers.Infrastructure;
using CoreKit.Modules.Tenancy.Infrastructure;
using CoreKit.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
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

    [Fact]
    public async Task CustomerService_UpdatesCustomer_WhenItExists()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "customers-update-test");
            await using var scope = provider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<ICustomerService>();

            var created = await service.CreateCustomerAsync(
                new CreateCustomerRequest("Before", "before@test.local"));
            var updated = await service.UpdateCustomerAsync(
                new UpdateCustomerRequest(created.Id, "After", "after@test.local"));

            Assert.NotNull(updated);
            Assert.Equal("After", updated.Name);
            Assert.Equal("after@test.local", updated.Email);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task RpcDispatcher_GetsAndUpdatesCustomer()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "customers-rpc-update-test");
            await using var scope = provider.CreateAsyncScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<RpcDispatcher>();

            var createResponse = await dispatcher.DispatchAsync(
                CreateRequest("customers.create", """{"name":"Wide World","email":"sales@wideworld.test"}"""));
            var createdCustomer = Assert.IsType<CoreKit.Modules.Customers.Application.CustomerDto>(createResponse.Data);

            var getResponse = await dispatcher.DispatchAsync(
                CreateRequest("customers.get", $$"""{"id":"{{createdCustomer.Id}}"}"""));
            var updateResponse = await dispatcher.DispatchAsync(
                CreateRequest(
                    "customers.update",
                    $$"""{"id":"{{createdCustomer.Id}}","name":"Wide World Updated","email":"updated@wideworld.test"}"""));

            Assert.True(getResponse.Succeeded);
            Assert.True(updateResponse.Succeeded);

            var loadedCustomer = Assert.IsType<CoreKit.Modules.Customers.Application.CustomerDto>(getResponse.Data);
            var updatedCustomer = Assert.IsType<CoreKit.Modules.Customers.Application.CustomerDto>(updateResponse.Data);

            Assert.Equal(createdCustomer.Id, loadedCustomer.Id);
            Assert.Equal("Wide World Updated", updatedCustomer.Name);
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
        services.AddScoped<IAuditEventWriter, NoOpAuditEventWriter>();
        services.AddScoped<ICurrentExecutionContextAccessor>(
            _ => new TestCurrentExecutionContextAccessor(
                new CurrentExecutionContext(
                    UserId: "test-user",
                    UserName: "test-user",
                    IsAuthenticated: true,
                    TenantIdentifier: tenantIdentifier)));
        services.AddScoped<ICurrentTenantAuthorizationService, AllowCurrentTenantAuthorizationService>();

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

    [Fact]
    public async Task CustomerService_RejectsDuplicateEmail_PerTenant()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "customers-unique-email-test");
            await using var scope = provider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<ICustomerService>();

            await service.CreateCustomerAsync(new CreateCustomerRequest("First", "same@test.local"));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateCustomerAsync(new CreateCustomerRequest("Second", "same@test.local")));

            Assert.Contains("unique", exception.Message, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task CustomerService_AllowsSameEmail_AcrossDifferentTenants()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var firstProvider = CreateServices(tempRoot, "tenant-one");
            await using var secondProvider = CreateServices(tempRoot, "tenant-two");
            await using var firstScope = firstProvider.CreateAsyncScope();
            await using var secondScope = secondProvider.CreateAsyncScope();

            var firstService = firstScope.ServiceProvider.GetRequiredService<ICustomerService>();
            var secondService = secondScope.ServiceProvider.GetRequiredService<ICustomerService>();

            await firstService.CreateCustomerAsync(new CreateCustomerRequest("Tenant One", "shared@test.local"));
            await secondService.CreateCustomerAsync(new CreateCustomerRequest("Tenant Two", "shared@test.local"));

            var firstCustomers = await firstService.GetCustomersAsync();
            var secondCustomers = await secondService.GetCustomersAsync();

            Assert.Single(firstCustomers);
            Assert.Single(secondCustomers);
            Assert.Equal("Tenant One", firstCustomers[0].Name);
            Assert.Equal("Tenant Two", secondCustomers[0].Name);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task CustomerService_DeletesCustomer_WhenItExists()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "customers-delete-test");
            await using var scope = provider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<ICustomerService>();

            var created = await service.CreateCustomerAsync(new CreateCustomerRequest("Delete Me", "delete@test.local"));
            var deleted = await service.DeleteCustomerAsync(created.Id);
            var customers = await service.GetCustomersAsync();

            Assert.True(deleted);
            Assert.Empty(customers);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task RpcDispatcher_DeletesCustomer()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "customers-rpc-delete-test");
            await using var scope = provider.CreateAsyncScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<RpcDispatcher>();

            var createResponse = await dispatcher.DispatchAsync(
                CreateRequest("customers.create", """{"name":"Delete Rpc","email":"delete-rpc@test.local"}"""));
            var createdCustomer = Assert.IsType<CoreKit.Modules.Customers.Application.CustomerDto>(createResponse.Data);

            var deleteResponse = await dispatcher.DispatchAsync(
                CreateRequest("customers.delete", $$"""{"id":"{{createdCustomer.Id}}"}"""));
            var listResponse = await dispatcher.DispatchAsync(CreateRequest("customers.list", "{}"));

            Assert.True(deleteResponse.Succeeded);
            Assert.True(Assert.IsType<bool>(deleteResponse.Data));

            var customers =
                Assert.IsAssignableFrom<IReadOnlyList<CoreKit.Modules.Customers.Application.CustomerDto>>(
                    listResponse.Data);

            Assert.Empty(customers);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task CustomerService_StampsAuditMetadata_OnCreateAndUpdate()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "customers-audit-test");
            await using var scope = provider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<ICustomerService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<CustomersDbContext>();

            var created = await service.CreateCustomerAsync(
                new CreateCustomerRequest("Audited", "audit@test.local"));
            var updated = await service.UpdateCustomerAsync(
                new UpdateCustomerRequest(created.Id, "Audited Updated", "audit-updated@test.local"));

            Assert.NotNull(updated);

            var entity = await dbContext.Customers.SingleAsync(customer => customer.Id == created.Id);

            Assert.Equal("customers-audit-test", entity.TenantIdentifier);
            Assert.Equal("test-user", entity.CreatedByUserId);
            Assert.Equal("test-user", entity.ModifiedByUserId);
            Assert.NotNull(entity.CreatedUtc);
            Assert.NotNull(entity.ModifiedUtc);
            Assert.True(entity.CreatedUtc <= entity.ModifiedUtc);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    private sealed class AllowCurrentTenantAuthorizationService : ICurrentTenantAuthorizationService
    {
        public Task<OperationError?> ValidateAccessAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<OperationError?>(null);

        public Task<OperationError?> ValidateAccessAsync(
            CurrentTenantAccessRequirement requirement,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<OperationError?>(null);
    }

    private sealed class TestCurrentExecutionContextAccessor(CurrentExecutionContext current)
        : ICurrentExecutionContextAccessor
    {
        public CurrentExecutionContext GetCurrent() => current;
    }
}
