using System.Security.Claims;
using CoreKit.BuildingBlocks.Presentation;
using CoreKit.Modules.Tenancy.Infrastructure;

namespace CoreKit.AppHost.Server.Diagnostics;

public sealed class LoggingAuditEventWriter(
    ILogger<LoggingAuditEventWriter> logger,
    IHttpContextAccessor httpContextAccessor,
    ITenantContextAccessor tenantContextAccessor) : IAuditEventWriter
{
    public Task WriteAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(auditEvent);

        var httpContext = httpContextAccessor.HttpContext;
        var user = httpContext?.User;
        var tenant = tenantContextAccessor.TenantContext?.Identifier ?? "host";
        var userName = user?.Identity?.IsAuthenticated == true
            ? user.FindFirstValue(ClaimTypes.Name) ?? user.Identity?.Name
            : null;

        using (logger.BeginScope(
                   new Dictionary<string, object?>
                   {
                       ["AuditCategory"] = auditEvent.Category,
                       ["AuditAction"] = auditEvent.Action,
                       ["AuditOutcome"] = auditEvent.Outcome,
                       ["TraceId"] = httpContext?.TraceIdentifier,
                       ["Tenant"] = tenant,
                       ["User"] = userName
                   }))
        {
            logger.LogInformation(
                "Audit {Category}/{Action} completed with outcome {Outcome}. Subject: {Subject}. Details: {@Details}",
                auditEvent.Category,
                auditEvent.Action,
                auditEvent.Outcome,
                auditEvent.Subject,
                auditEvent.Details);
        }

        return Task.CompletedTask;
    }
}
