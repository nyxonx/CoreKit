using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CoreKit.PlatformAppHost.Server.Diagnostics;

public sealed class RpcOperationsHealthCheck(Rpc.RpcOperationRegistry operationRegistry) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var operationCount = operationRegistry.Count;

        return Task.FromResult(
            operationCount == 0
                ? HealthCheckResult.Unhealthy("No RPC operations are registered.")
                : HealthCheckResult.Healthy(
                    "RPC operations are registered.",
                    new Dictionary<string, object>
                    {
                        ["operationCount"] = operationCount
                    }));
    }
}
