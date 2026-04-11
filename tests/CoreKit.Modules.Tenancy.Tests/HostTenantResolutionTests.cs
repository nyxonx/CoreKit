using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using Xunit;

namespace CoreKit.Modules.Tenancy.Tests;

public sealed class HostTenantResolutionTests
{
    [Fact(Skip = "Manual host verification is currently preferred in this environment.")]
    public async Task AppHost_ResolvesSeededTenant_ThroughHttpPipeline()
    {
        var solutionRoot = FindSolutionRoot();
        var serverProject = Path.Combine(solutionRoot, "src", "CoreKit.AppHost.Server", "CoreKit.AppHost.Server.csproj");
        var serverDll = Path.Combine(solutionRoot, "src", "CoreKit.AppHost.Server", "bin", "Debug", "net10.0", "CoreKit.AppHost.Server.dll");
        var serverUrl = $"http://127.0.0.1:{GetFreeTcpPort()}";
        var tempRoot = Path.Combine(Path.GetTempPath(), "corekit-tenancy-tests", Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(tempRoot);

        var tenantCatalogPath = Path.Combine(tempRoot, "catalog.db");
        var tenantDataPath = Path.Combine(tempRoot, "tenant.db");
        var identityPath = Path.Combine(tempRoot, "identity.db");

        await BuildServerAsync(solutionRoot, serverProject);

        using var process = StartServerProcess(
            solutionRoot,
            serverDll,
            serverUrl,
            tenantCatalogPath,
            tenantDataPath,
            identityPath);

        try
        {
            await WaitForHealthyServerAsync(serverUrl, process);

            using var httpClient = new HttpClient { BaseAddress = new Uri(serverUrl) };
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/modules/tenancy/status");
            request.Headers.Host = "localhost";

            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<TenancyStatusResponse>();
            Assert.NotNull(payload);
            Assert.Equal("Tenancy", payload.module);
            Assert.Equal("registered", payload.status);
            Assert.NotNull(payload.tenant);
            Assert.Equal("bootstrap", payload.tenant.Identifier);
            Assert.Equal("localhost", payload.tenant.Host);
        }
        finally
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                    await process.WaitForExitAsync();
                }
            }
            finally
            {
                if (Directory.Exists(tempRoot))
                {
                    Directory.Delete(tempRoot, recursive: true);
                }
            }
        }
    }

    private static Process StartServerProcess(
        string solutionRoot,
        string serverDll,
        string serverUrl,
        string tenantCatalogPath,
        string tenantDataPath,
        string identityPath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"\"{serverDll}\" --urls \"{serverUrl}\"",
            WorkingDirectory = solutionRoot,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
        startInfo.Environment["ConnectionStrings__TenantCatalogDatabase"] = $"Data Source={tenantCatalogPath}";
        startInfo.Environment["ConnectionStrings__DefaultTenantDatabase"] = $"Data Source={tenantDataPath}";
        startInfo.Environment["ConnectionStrings__IdentityDatabase"] = $"Data Source={identityPath}";
        startInfo.Environment["Tenancy__Seed__Identifier"] = "bootstrap";
        startInfo.Environment["Tenancy__Seed__Name"] = "Bootstrap Tenant";
        startInfo.Environment["Tenancy__Seed__Host"] = "localhost";

        var process = new Process { StartInfo = startInfo };
        process.Start();

        return process;
    }

    private static async Task BuildServerAsync(string solutionRoot, string serverProject)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{serverProject}\" -m:1 /nologo",
                WorkingDirectory = solutionRoot,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        process.Start();
        var standardOutput = await process.StandardOutput.ReadToEndAsync();
        var standardError = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Failed to build AppHost server for host test.{Environment.NewLine}{standardOutput}{Environment.NewLine}{standardError}");
        }
    }

    private static async Task WaitForHealthyServerAsync(string serverUrl, Process? process = null)
    {
        using var httpClient = new HttpClient { BaseAddress = new Uri(serverUrl) };
        var output = new StringBuilder();

        for (var attempt = 0; attempt < 60; attempt++)
        {
            if (process is not null && process.HasExited)
            {
                var standardOutput = await process.StandardOutput.ReadToEndAsync();
                var standardError = await process.StandardError.ReadToEndAsync();

                throw new InvalidOperationException(
                    $"The AppHost server exited before becoming healthy.{Environment.NewLine}{standardOutput}{Environment.NewLine}{standardError}");
            }

            try
            {
                using var response = await httpClient.GetAsync("/health");

                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch (HttpRequestException)
            {
            }

            await Task.Delay(500);
        }

        if (process is not null)
        {
            output.AppendLine(await process.StandardOutput.ReadToEndAsync());
            output.AppendLine(await process.StandardError.ReadToEndAsync());
        }

        throw new TimeoutException(
            $"The AppHost server did not become healthy in time.{Environment.NewLine}{output}");
    }

    private static int GetFreeTcpPort()
    {
        var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();

        try
        {
            return ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
        }
        finally
        {
            listener.Stop();
        }
    }

    private static string FindSolutionRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "CoreKit.sln");

            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate the solution root.");
    }

    private sealed record TenancyStatusResponse(string module, string status, TenantPayload? tenant);

    private sealed record TenantPayload(string Identifier, string Name, string Host);
}
