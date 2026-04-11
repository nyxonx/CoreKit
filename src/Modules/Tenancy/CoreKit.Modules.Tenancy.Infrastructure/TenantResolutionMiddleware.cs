using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantResolutionMiddleware(RequestDelegate next)
{
    private static readonly string[] ExcludedPrefixes =
    [
        "/_framework",
        "/_vs",
        "/css",
        "/favicon",
        "/icon-",
        "/lib",
        "/manifest.webmanifest",
        "/sample-data"
    ];

    public async Task InvokeAsync(
        HttpContext context,
        TenantResolutionService tenantResolutionService,
        ITenantContextAccessor tenantContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(tenantResolutionService);
        ArgumentNullException.ThrowIfNull(tenantContextAccessor);

        if (ShouldSkip(context.Request.Path))
        {
            await next(context);
            return;
        }

        var resolutionResult = await tenantResolutionService.ResolveAsync(context, context.RequestAborted);

        if (!resolutionResult.IsResolved || resolutionResult.Tenant is null)
        {
            context.Response.StatusCode =
                resolutionResult.FailureReason?.Contains("inactive", StringComparison.OrdinalIgnoreCase) == true
                    ? StatusCodes.Status403Forbidden
                    : StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(
                    new
                    {
                        error = "tenant_resolution_failed",
                        detail = resolutionResult.FailureReason
                    }),
                context.RequestAborted);

            return;
        }

        tenantContextAccessor.TenantContext = new TenantContext
        {
            Identifier = resolutionResult.Tenant.Identifier,
            Name = resolutionResult.Tenant.Name,
            Host = resolutionResult.Tenant.Host,
            ConnectionString = resolutionResult.Tenant.ConnectionString
        };

        context.Items[typeof(TenantContext)] = tenantContextAccessor.TenantContext;

        await next(context);
    }

    private static bool ShouldSkip(PathString path)
    {
        if (!path.HasValue)
        {
            return true;
        }

        if (path == "/health" || path == "/")
        {
            return true;
        }

        return ExcludedPrefixes.Any(prefix => path.StartsWithSegments(prefix, StringComparison.OrdinalIgnoreCase));
    }
}
