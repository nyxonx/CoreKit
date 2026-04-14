using CoreKit.BuildingBlocks.Presentation;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CoreKit.AppHost.Server.Diagnostics;

public sealed class BackgroundJobsHealthCheck(CoreKitBackgroundJobRegistry registry) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            HealthCheckResult.Healthy(
                "Background jobs are registered.",
                new Dictionary<string, object>
                {
                    ["jobCount"] = registry.Jobs.Count
                }));
    }
}
