using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CoreKit.AppHost.Contracts.Authentication;
using CoreKit.AppHost.Contracts.Rpc;
using CoreKit.BuildingBlocks.Application;
using CoreKit.Modules.Identity.Infrastructure;
using CoreKit.Modules.Tenancy.Domain;
using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreKit.Modules.Customers.Tests;

public sealed class CustomersAuthorizationTests
{
    [Fact]
    public async Task CustomersRpc_ReturnsAuthenticationRequired_WhenUserIsAnonymous()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var factory = new CoreKitAppFactory(tempRoot);
            using var client = factory.CreateClient();

            using var response = await SendRpcAsync(client, "localhost", "customers.list", "{}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            var payload = await response.Content.ReadFromJsonAsync<RpcResponse>();
            Assert.NotNull(payload);
            Assert.False(payload.Succeeded);
            Assert.Contains(payload.Errors, error => error.Code == "authentication_required");
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task CustomersRpc_Succeeds_ForBootstrapTenantMember()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var factory = new CoreKitAppFactory(tempRoot);
            using var client = factory.CreateClient();
            var authCookie = await LoginAsync(client, "localhost");

            using var response = await SendRpcAsync(client, "localhost", "customers.list", "{}", authCookie);

            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<RpcResponse>();
            Assert.NotNull(payload);
            Assert.True(payload.Succeeded);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task AuthState_ReturnsResolvedTenantAndTenantRole_ForAuthenticatedUser()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var factory = new CoreKitAppFactory(tempRoot);
            using var client = factory.CreateClient();
            var authCookie = await LoginAsync(client, "localhost");

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/state");
            request.Headers.Host = "localhost";
            request.Headers.Add("Cookie", authCookie);

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<AuthStateResponse>();
            Assert.NotNull(payload);
            Assert.True(payload.IsAuthenticated);
            Assert.Equal("bootstrap", payload.TenantIdentifier);
            Assert.Equal(TenantMembershipRoles.Admin, payload.TenantRole);
            Assert.False(payload.IsControlPlaneHost);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task TenantCatalogRpc_Succeeds_ForGlobalAdmin_OnControlPlaneHost()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var factory = new CoreKitAppFactory(tempRoot);
            using var client = factory.CreateClient();
            var authCookie = await LoginAsync(client, "admin.local");

            using var response = await SendRpcAsync(client, "admin.local", "tenancy.catalog.list", "{}", authCookie);

            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<RpcResponse>();
            Assert.NotNull(payload);
            Assert.True(payload.Succeeded);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task TenantCreateRpc_ProvisionsTenant_ForGlobalAdmin_OnControlPlaneHost()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var factory = new CoreKitAppFactory(tempRoot);
            using var client = factory.CreateClient();
            var authCookie = await LoginAsync(client, "admin.local");

            using var response = await SendRpcAsync(
                client,
                "admin.local",
                "tenancy.catalog.create",
                """{"identifier":"acme","name":"Acme Corp","host":"acme.local"}""",
                authCookie);

            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<RpcResponse>();
            Assert.NotNull(payload);
            Assert.True(payload.Succeeded);

            await using var scope = factory.Services.CreateAsyncScope();
            var catalogDbContext = scope.ServiceProvider.GetRequiredService<TenantCatalogDbContext>();
            var tenant = await catalogDbContext.Tenants.SingleOrDefaultAsync(entity => entity.Identifier == "acme");
            Assert.NotNull(tenant);
            Assert.Equal("acme.local", tenant.Host);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task CustomersRpc_ReturnsForbidden_WhenUserLacksMembershipForResolvedTenant()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var factory = new CoreKitAppFactory(tempRoot);
            await factory.AddTenantAsync("secondary", "secondary.local", Path.Combine(tempRoot, "secondary.db"));

            using var client = factory.CreateClient();
            var authCookie = await LoginAsync(client, "localhost");

            using var response = await SendRpcAsync(client, "secondary.local", "customers.list", "{}", authCookie);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            var payload = await response.Content.ReadFromJsonAsync<RpcResponse>();
            Assert.NotNull(payload);
            Assert.False(payload.Succeeded);
            Assert.Contains(payload.Errors, error => error.Code == "tenant_membership_required");
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task CustomersRpc_ReturnsForbidden_WhenMemberAttemptsWriteOperation()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var factory = new CoreKitAppFactory(tempRoot);
            await factory.SetMembershipRoleAsync("admin", "bootstrap", TenantMembershipRoles.Member);

            using var client = factory.CreateClient();
            var authCookie = await LoginAsync(client, "localhost");

            using var response = await SendRpcAsync(
                client,
                "localhost",
                "customers.create",
                """{"name":"Blocked Write","email":"blocked@test.local"}""",
                authCookie);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            var payload = await response.Content.ReadFromJsonAsync<RpcResponse>();
            Assert.NotNull(payload);
            Assert.False(payload.Succeeded);
            Assert.Contains(payload.Errors, error => error.Code == "tenant_role_required");
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task IdentityMembershipRpc_AllowsAdminToChangeTenantRole()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var factory = new CoreKitAppFactory(tempRoot);
            using var client = factory.CreateClient();
            var authCookie = await LoginAsync(client, "localhost");

            using var updateResponse = await SendRpcAsync(
                client,
                "localhost",
                "identity.memberships.upsert",
                """{"userName":"admin","role":"Member"}""",
                authCookie);

            updateResponse.EnsureSuccessStatusCode();

            using var createResponse = await SendRpcAsync(
                client,
                "localhost",
                "customers.create",
                """{"name":"Should Fail","email":"should-fail@test.local"}""",
                authCookie);

            Assert.Equal(HttpStatusCode.Forbidden, createResponse.StatusCode);

            var payload = await createResponse.Content.ReadFromJsonAsync<RpcResponse>();
            Assert.NotNull(payload);
            Assert.Contains(payload.Errors, error => error.Code == "tenant_role_required");
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    private static async Task<string> LoginAsync(HttpClient client, string host)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login")
        {
            Content = JsonContent.Create(new LoginRequest("admin", "Admin1234", false))
        };
        request.Headers.Host = host;

        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var setCookie = response.Headers.TryGetValues("Set-Cookie", out var values)
            ? values.FirstOrDefault()
            : null;

        Assert.False(string.IsNullOrWhiteSpace(setCookie));

        return setCookie!.Split(';', 2)[0];
    }

    private static async Task<HttpResponseMessage> SendRpcAsync(
        HttpClient client,
        string host,
        string operation,
        string payloadJson,
        string? authCookie = null)
    {
        using var document = JsonDocument.Parse(payloadJson);
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/rpc")
        {
            Content = JsonContent.Create(new RpcRequest(operation, document.RootElement.Clone()))
        };

        request.Headers.Host = host;

        if (!string.IsNullOrWhiteSpace(authCookie))
        {
            request.Headers.Add("Cookie", authCookie);
        }

        return await client.SendAsync(request);
    }

    private static string CreateTempRoot()
    {
        var tempRoot = Path.Combine(Path.GetTempPath(), "corekit-customers-auth-tests", Guid.NewGuid().ToString("N"));
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

    private sealed class CoreKitAppFactory(string tempRoot) : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var tenantCatalogPath = Path.Combine(tempRoot, "catalog.db");
            var tenantDataPath = Path.Combine(tempRoot, "tenant.db");
            var identityPath = Path.Combine(tempRoot, "identity.db");

            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration(
                (_, configuration) =>
                {
                    configuration.AddInMemoryCollection(
                    [
                        KeyValuePair.Create<string, string?>("ConnectionStrings:TenantCatalogDatabase", $"Data Source={tenantCatalogPath}"),
                        KeyValuePair.Create<string, string?>("ConnectionStrings:DefaultTenantDatabase", $"Data Source={tenantDataPath}"),
                        KeyValuePair.Create<string, string?>("ConnectionStrings:IdentityDatabase", $"Data Source={identityPath}"),
                        KeyValuePair.Create<string, string?>("Tenancy:Seed:Identifier", "bootstrap"),
                        KeyValuePair.Create<string, string?>("Tenancy:Seed:Name", "Bootstrap Tenant"),
                        KeyValuePair.Create<string, string?>("Tenancy:Seed:Host", "localhost"),
                        KeyValuePair.Create<string, string?>("Tenancy:ControlPlaneHosts:0", "admin.local")
                    ]);
                });
        }

        public async Task AddTenantAsync(string identifier, string host, string databasePath)
        {
            await using var scope = Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TenantCatalogDbContext>();

            var exists = await dbContext.Tenants.AnyAsync(tenant => tenant.Identifier == identifier);

            if (exists)
            {
                return;
            }

            dbContext.Tenants.Add(
                new TenantCatalogEntry
                {
                    Identifier = identifier,
                    Name = identifier,
                    Host = host,
                    ConnectionString = $"Data Source={databasePath}",
                    IsActive = true
                });

            await dbContext.SaveChangesAsync();
        }

        public async Task SetMembershipRoleAsync(string userName, string tenantIdentifier, string role)
        {
            await using var scope = Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            var user = await dbContext.Users.SingleAsync(entity => entity.UserName == userName);
            var membership = await dbContext.UserTenantMemberships.SingleAsync(
                entity => entity.UserId == user.Id && entity.TenantIdentifier == tenantIdentifier);

            membership.Role = role;
            await dbContext.SaveChangesAsync();
        }
    }
}
