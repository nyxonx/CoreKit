using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CoreKit.AppHost.Server.Diagnostics;

public sealed class TenantCatalogHealthCheck(TenantCatalogDbContext tenantCatalogDbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var canConnect = await tenantCatalogDbContext.Database.CanConnectAsync(cancellationToken);

        if (!canConnect)
        {
            return HealthCheckResult.Unhealthy("Tenant catalog database is not reachable.");
        }

        var tenantCount = await tenantCatalogDbContext.Tenants.CountAsync(cancellationToken);

        return HealthCheckResult.Healthy(
            "Tenant catalog database is reachable.",
            new Dictionary<string, object>
            {
                ["tenantCount"] = tenantCount
            });
    }
}
