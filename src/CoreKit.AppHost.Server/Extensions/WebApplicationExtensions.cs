using System.Text.Json;
using CoreKit.AppHost.Server.Diagnostics;
using CoreKit.AppHost.Server.Rpc;
using CoreKit.BuildingBlocks.Presentation;
using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CoreKit.AppHost.Server.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseCoreKitAppHost(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
            app.UseHsts();
        }

        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        app.UseMiddleware<SecurityHeadersMiddleware>();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseMiddleware<TenantResolutionMiddleware>();
        app.UseMiddleware<RequestContextLoggingMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static WebApplication MapCoreKitInfrastructureEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapGet(
                "/",
                () => Results.Ok(
                    new
                    {
                        name = "CoreKit AppHost Server",
                        status = "ok",
                        environment = app.Environment.EnvironmentName
                    }))
            .WithName("GetServerRoot")
            .WithTags("System");

        app.MapGet(
                "/api/system/info",
                () => Results.Ok(
                    new
                    {
                        application = "CoreKit",
                        host = "Server",
                        environment = app.Environment.EnvironmentName
                    }))
            .WithName("GetSystemInfo")
            .WithTags("System");

        app.MapGet(
                "/api/system/runtime",
                (
                    IHostEnvironment environment,
                    RpcOperationRegistry rpcOperationRegistry,
                    CoreKitBackgroundJobRegistry backgroundJobRegistry) => Results.Ok(
                    new
                    {
                        application = "CoreKit",
                        environment = environment.EnvironmentName,
                        utcNow = DateTimeOffset.UtcNow,
                        rpcOperations = rpcOperationRegistry.Count,
                        backgroundJobs = backgroundJobRegistry.Jobs.Select(
                            job => new
                            {
                                job.Name,
                                interval = job.Interval.ToString()
                            }),
                        metrics = new
                        {
                            meter = CoreKitTelemetry.MeterName
                        }
                    }))
            .WithName("GetRuntimeInfo")
            .WithTags("System");

        app.MapHealthChecks(
                "/health",
                new HealthCheckOptions
                {
                    ResponseWriter = WriteHealthResponseAsync
                })
            .WithTags("System");
        app.MapRpcEndpoints();
        app.MapCoreKitModules();
        app.MapFallbackToFile("index.html");

        return app;
    }

    private static Task WriteHealthResponseAsync(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var payload = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(
                entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    description = entry.Value.Description,
                    duration = entry.Value.Duration.TotalMilliseconds,
                    data = entry.Value.Data
                })
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
