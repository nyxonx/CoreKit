using System.Security.Claims;
using CoreKit.BuildingBlocks.Application;
using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace CoreKit.Modules.Identity.Infrastructure;

public sealed class HttpContextCurrentExecutionContextAccessor(
    IHttpContextAccessor httpContextAccessor,
    ITenantContextAccessor tenantContextAccessor) : ICurrentExecutionContextAccessor
{
    public CurrentExecutionContext GetCurrent()
    {
        var principal = httpContextAccessor.HttpContext?.User;

        return new CurrentExecutionContext(
            principal?.FindFirstValue(ClaimTypes.NameIdentifier),
            principal?.FindFirstValue(ClaimTypes.Name) ?? principal?.Identity?.Name,
            principal?.Identity?.IsAuthenticated == true,
            tenantContextAccessor.TenantContext?.Identifier,
            principal?.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToArray() ?? Array.Empty<string>());
    }
}
