using System.Net;
using System.Net.Http.Json;
using CoreKit.AppHost.Contracts.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CoreKit.Modules.Tenancy.Tests;

public sealed class HostTenantResolutionTests
{
    [Fact]
    public async Task AuthState_ReturnsTenantResolutionFailure_BeforeAuthStateEvaluation_WhenTenantIsUnknown()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var factory = new CoreKitAppFactory(tempRoot);
            using var client = factory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/state");
            request.Headers.Host = "unknown.local";

            using var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var payload = await response.Content.ReadFromJsonAsync<TenantResolutionFailureResponse>();
            Assert.NotNull(payload);
            Assert.Equal("tenant_resolution_failed", payload.error);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    [Fact]
    public async Task AuthState_ReturnsUnauthenticatedPayload_WhenTenantIsResolved()
    {
        var tempRoot = CreateTempRoot();

        try
        {
            await using var factory = new CoreKitAppFactory(tempRoot);
            using var client = factory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/state");
            request.Headers.Host = "localhost";

            using var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<AuthStateResponse>();
            Assert.NotNull(payload);
            Assert.False(payload.IsAuthenticated);
            Assert.Null(payload.UserName);
            Assert.Empty(payload.Roles);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    private static string CreateTempRoot()
    {
        var tempRoot = Path.Combine(Path.GetTempPath(), "corekit-tenancy-tests", Guid.NewGuid().ToString("N"));
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
                        KeyValuePair.Create<string, string?>("Tenancy:Seed:Host", "localhost")
                    ]);
                });
        }
    }

    private sealed record TenantResolutionFailureResponse(string error, string? detail);
}
