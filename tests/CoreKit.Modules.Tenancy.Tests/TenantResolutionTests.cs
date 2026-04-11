using System.Text.Json;
using CoreKit.Modules.Tenancy.Domain;
using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CoreKit.Modules.Tenancy.Tests;

public sealed class TenantResolutionTests
{
    [Fact]
    public async Task ResolveAsync_UsesHostWhenHeaderIsMissing()
    {
        using var connection = CreateOpenConnection();
        await using var dbContext = CreateDbContext(connection);

        await SeedTenantAsync(
            dbContext,
            new TenantCatalogEntry
            {
                Identifier = "bootstrap",
                Name = "Bootstrap Tenant",
                Host = "localhost",
                IsActive = true,
                ConnectionString = "Data Source=bootstrap.db"
            });

        var service = new TenantResolutionService(dbContext);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Host = new HostString("localhost");

        var result = await service.ResolveAsync(httpContext);

        Assert.True(result.IsResolved);
        Assert.NotNull(result.Tenant);
        Assert.Equal("bootstrap", result.Tenant.Identifier);
    }

    [Fact]
    public async Task ResolveAsync_PrefersHeaderOverHost()
    {
        using var connection = CreateOpenConnection();
        await using var dbContext = CreateDbContext(connection);

        await SeedTenantAsync(
            dbContext,
            new TenantCatalogEntry
            {
                Identifier = "bootstrap",
                Name = "Bootstrap Tenant",
                Host = "localhost",
                IsActive = true,
                ConnectionString = "Data Source=bootstrap.db"
            },
            new TenantCatalogEntry
            {
                Identifier = "contoso",
                Name = "Contoso",
                Host = "contoso.local",
                IsActive = true,
                ConnectionString = "Data Source=contoso.db"
            });

        var service = new TenantResolutionService(dbContext);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Host = new HostString("localhost");
        httpContext.Request.Headers["X-Tenant"] = "contoso";

        var result = await service.ResolveAsync(httpContext);

        Assert.True(result.IsResolved);
        Assert.NotNull(result.Tenant);
        Assert.Equal("contoso", result.Tenant.Identifier);
    }

    [Fact]
    public async Task Middleware_Returns404_WhenTenantDoesNotExist()
    {
        using var connection = CreateOpenConnection();
        await using var dbContext = CreateDbContext(connection);

        var service = new TenantResolutionService(dbContext);
        var accessor = new TenantContextAccessor();
        var middleware = new TenantResolutionMiddleware(_ => Task.CompletedTask);
        var httpContext = CreateRequestContext("missing.local");

        await middleware.InvokeAsync(httpContext, service, accessor);

        Assert.Equal(StatusCodes.Status404NotFound, httpContext.Response.StatusCode);

        var payload = await ReadJsonPayloadAsync(httpContext);
        Assert.Equal("tenant_resolution_failed", payload.error);
        Assert.Contains("Unknown tenant host", payload.detail, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Middleware_Returns403_WhenTenantIsInactive()
    {
        using var connection = CreateOpenConnection();
        await using var dbContext = CreateDbContext(connection);

        await SeedTenantAsync(
            dbContext,
            new TenantCatalogEntry
            {
                Identifier = "inactive",
                Name = "Inactive Tenant",
                Host = "inactive.local",
                IsActive = false,
                ConnectionString = "Data Source=inactive.db"
            });

        var service = new TenantResolutionService(dbContext);
        var accessor = new TenantContextAccessor();
        var middleware = new TenantResolutionMiddleware(_ => Task.CompletedTask);
        var httpContext = CreateRequestContext("inactive.local");

        await middleware.InvokeAsync(httpContext, service, accessor);

        Assert.Equal(StatusCodes.Status403Forbidden, httpContext.Response.StatusCode);

        var payload = await ReadJsonPayloadAsync(httpContext);
        Assert.Equal("tenant_resolution_failed", payload.error);
        Assert.Contains("inactive", payload.detail, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Middleware_SetsTenantContextAndInvokesNext_WhenTenantIsResolved()
    {
        using var connection = CreateOpenConnection();
        await using var dbContext = CreateDbContext(connection);

        await SeedTenantAsync(
            dbContext,
            new TenantCatalogEntry
            {
                Identifier = "bootstrap",
                Name = "Bootstrap Tenant",
                Host = "localhost",
                IsActive = true,
                ConnectionString = "Data Source=bootstrap.db"
            });

        var service = new TenantResolutionService(dbContext);
        var accessor = new TenantContextAccessor();
        var wasNextCalled = false;
        var middleware = new TenantResolutionMiddleware(
            _ =>
            {
                wasNextCalled = true;
                return Task.CompletedTask;
            });
        var httpContext = CreateRequestContext("localhost");

        await middleware.InvokeAsync(httpContext, service, accessor);

        Assert.True(wasNextCalled);
        Assert.NotNull(accessor.TenantContext);
        Assert.Equal("bootstrap", accessor.TenantContext.Identifier);
        Assert.Equal(StatusCodes.Status200OK, httpContext.Response.StatusCode);
    }

    private static SqliteConnection CreateOpenConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        return connection;
    }

    private static TenantCatalogDbContext CreateDbContext(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<TenantCatalogDbContext>()
            .UseSqlite(connection)
            .Options;

        var dbContext = new TenantCatalogDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }

    private static async Task SeedTenantAsync(
        TenantCatalogDbContext dbContext,
        params TenantCatalogEntry[] tenants)
    {
        dbContext.Tenants.AddRange(tenants);
        await dbContext.SaveChangesAsync();
    }

    private static DefaultHttpContext CreateRequestContext(string host)
    {
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/modules/tenancy/status";
        context.Request.Host = new HostString(host);
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<TenantErrorResponse> ReadJsonPayloadAsync(DefaultHttpContext context)
    {
        context.Response.Body.Position = 0;
        var payload = await JsonSerializer.DeserializeAsync<TenantErrorResponse>(context.Response.Body);
        return payload ?? throw new InvalidOperationException("Expected tenant error payload.");
    }

    private sealed record TenantErrorResponse(string error, string detail);
}
