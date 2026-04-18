using CoreKit.Modules.Tenancy.Infrastructure;

namespace CoreKit.PlatformAppHost.Server.Diagnostics;

public sealed class RequestContextLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestContextLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext, ITenantContextAccessor tenantContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(tenantContextAccessor);

        var tenantIdentifier = tenantContextAccessor.TenantContext?.Identifier ?? "host";

        using (logger.BeginScope(
                   new Dictionary<string, object?>
                   {
                       ["TraceId"] = httpContext.TraceIdentifier,
                       ["Tenant"] = tenantIdentifier
                   }))
        {
            logger.LogInformation(
                "HTTP {Method} {Path} started for tenant {Tenant}.",
                httpContext.Request.Method,
                httpContext.Request.Path,
                tenantIdentifier);

            try
            {
                await next(httpContext);

                logger.LogInformation(
                    "HTTP {Method} {Path} completed with status {StatusCode}.",
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    httpContext.Response.StatusCode);
            }
            catch (Exception)
            {
                logger.LogWarning(
                    "HTTP {Method} {Path} failed before a response was written.",
                    httpContext.Request.Method,
                    httpContext.Request.Path);
                throw;
            }
        }
    }
}
