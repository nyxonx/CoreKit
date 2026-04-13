using System.Text.Json;
using CoreKit.AppHost.Contracts.Rpc;
using CoreKit.AppHost.Server.Rpc;
using CoreKit.BuildingBlocks.Application;
using CoreKit.Modules.Tenancy.Application;
using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreKit.Modules.Tenancy.Tests;

public sealed class RpcDispatcherTests
{
    [Fact]
    public async Task DispatchAsync_AddTenantNoteCommand_ReturnsCreatedNote()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "rpc-note-test");
            await using var scope = provider.CreateAsyncScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<RpcDispatcher>();
            var request = CreateRequest("tenancy.notes.add", """{"value":"rpc note"}""");

            var response = await dispatcher.DispatchAsync(request);

            Assert.True(response.Succeeded);
            Assert.Empty(response.Errors);

            var payload = Assert.IsType<TenantNoteDto>(response.Data);
            Assert.Equal("rpc note", payload.Value);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task DispatchAsync_ReturnsValidationError_ForEmptyTenantNote()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "rpc-validation-test");
            await using var scope = provider.CreateAsyncScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<RpcDispatcher>();
            var request = CreateRequest("tenancy.notes.add", """{"value":""}""");

            var response = await dispatcher.DispatchAsync(request);

            Assert.False(response.Succeeded);
            Assert.Single(response.Errors);
            Assert.Equal("validation_error", response.Errors[0].Code);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task DispatchAsync_GetTenantNotesQuery_ReturnsTenantScopedNotes()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var provider = CreateServices(tempRoot, "rpc-query-test");
            await using var scope = provider.CreateAsyncScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<RpcDispatcher>();

            await dispatcher.DispatchAsync(CreateRequest("tenancy.notes.add", """{"value":"first"}"""));
            await dispatcher.DispatchAsync(CreateRequest("tenancy.notes.add", """{"value":"second"}"""));

            var response = await dispatcher.DispatchAsync(CreateRequest("tenancy.notes.list", "{}"));

            Assert.True(response.Succeeded);
            Assert.Empty(response.Errors);

            var notes = Assert.IsAssignableFrom<IReadOnlyList<TenantNoteDto>>(response.Data);
            Assert.Equal(2, notes.Count);
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
        services.AddCoreKitApplication(typeof(TenancyApplicationAssemblyMarker).Assembly);
        services.AddScoped<ITenantNoteService, TenantNoteService>();
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
        services.AddScoped<ITenantDbContextFactory, TenantDbContextFactory>();
        services.AddScoped(
            serviceProvider => serviceProvider.GetRequiredService<ITenantDbContextFactory>().CreateDbContext());
        services.AddSingleton(new RpcOperationRegistry(typeof(TenancyApplicationAssemblyMarker).Assembly));
        services.AddScoped<RpcDispatcher>();

        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TenantAppDbContext>();
        dbContext.Database.EnsureCreated();

        return provider;
    }

    private static RpcRequest CreateRequest(string operation, string payloadJson)
    {
        using var document = JsonDocument.Parse(payloadJson);
        return new RpcRequest(operation, document.RootElement.Clone());
    }

    private static string CreateTempRoot()
    {
        var tempRoot = Path.Combine(Path.GetTempPath(), "corekit-rpc-tests", Guid.NewGuid().ToString("N"));
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
