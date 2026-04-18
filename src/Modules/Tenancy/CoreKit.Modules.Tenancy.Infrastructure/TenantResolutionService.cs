using CoreKit.Modules.Tenancy.Application;
using CoreKit.Modules.Tenancy.Domain;
using Microsoft.AspNetCore.Http;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantResolutionService(
    ITenantRegistry tenantRegistry)
{
    public async Task<TenantResolutionResult> ResolveAsync(
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var headerIdentifier = httpContext.Request.Headers["X-Tenant"].FirstOrDefault()?.Trim();

        if (!string.IsNullOrWhiteSpace(headerIdentifier))
        {
            var tenantByIdentifier = await tenantRegistry.GetByIdentifierAsync(headerIdentifier, cancellationToken);

            return CreateResult(tenantByIdentifier, $"Unknown tenant '{headerIdentifier}'.");
        }

        var host = httpContext.Request.Host.Host;

        if (string.IsNullOrWhiteSpace(host))
        {
            return TenantResolutionResult.Failure("Tenant could not be resolved because the request host was missing.");
        }

        var tenantByHost = await tenantRegistry.GetByHostAsync(host, cancellationToken);

        return CreateResult(tenantByHost, $"Unknown tenant host '{host}'.");
    }

    private static TenantResolutionResult CreateResult(
        TenantRuntimeInfo? tenant,
        string missingTenantMessage)
    {
        if (tenant is null)
        {
            return TenantResolutionResult.Failure(missingTenantMessage);
        }

        if (!tenant.IsActive)
        {
            return TenantResolutionResult.Failure($"Tenant '{tenant.Identifier}' is inactive.");
        }

        return TenantResolutionResult.Success(tenant);
    }
}
