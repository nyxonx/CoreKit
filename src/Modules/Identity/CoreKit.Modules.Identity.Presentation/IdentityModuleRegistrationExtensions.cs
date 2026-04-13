using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoreKit.AppHost.Contracts.Authentication;
using CoreKit.BuildingBlocks.Presentation;
using CoreKit.Modules.Identity.Domain;
using CoreKit.Modules.Identity.Infrastructure;
using CoreKit.Modules.Tenancy.Infrastructure;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace CoreKit.Modules.Identity.Presentation;

public static class IdentityModuleRegistrationExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services;
    }

    public static IEndpointRouteBuilder MapIdentityModule(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/api/auth").WithTags("Identity");

        group.MapGet(
            "/status",
            () => Results.Ok(
                new
                {
                    module = "Identity",
                    status = "registered"
                }));

        group.MapPost(
            "/login",
            async (
                LoginRequest request,
                SignInManager<AppUser> signInManager,
                IAuditEventWriter auditEventWriter,
                CancellationToken cancellationToken) =>
            {
                var result = await signInManager.PasswordSignInAsync(
                    request.UserName,
                    request.Password,
                    request.RememberMe,
                    lockoutOnFailure: false);

                var outcome = result.Succeeded ? "success" : "failure";
                await auditEventWriter.WriteAsync(
                    new AuditEvent(
                        "identity",
                        "login",
                        outcome,
                        request.UserName,
                        new Dictionary<string, object?>
                        {
                            ["rememberMe"] = request.RememberMe
                        }),
                    cancellationToken);
                CoreKitTelemetry.AuthEvents.Add(1, new KeyValuePair<string, object?>("action", "login"), new KeyValuePair<string, object?>("outcome", outcome));

                return result.Succeeded
                    ? Results.Ok()
                    : Results.Unauthorized();
            });

        group.MapPost(
            "/logout",
            async (
                ClaimsPrincipal principal,
                SignInManager<AppUser> signInManager,
                IAuditEventWriter auditEventWriter,
                CancellationToken cancellationToken) =>
            {
                await signInManager.SignOutAsync();
                var subject = principal.Identity?.Name;
                await auditEventWriter.WriteAsync(
                    new AuditEvent("identity", "logout", "success", subject),
                    cancellationToken);
                CoreKitTelemetry.AuthEvents.Add(1, new KeyValuePair<string, object?>("action", "logout"), new KeyValuePair<string, object?>("outcome", "success"));
                return Results.Ok();
            });

        group.MapGet(
            "/state",
            async (
                HttpContext httpContext,
                ClaimsPrincipal principal,
                UserManager<AppUser> userManager,
                AppIdentityDbContext identityDbContext,
                ITenantContextAccessor tenantContextAccessor,
                IOptions<ControlPlaneHostOptions> controlPlaneHostOptions) =>
            {
                ApplyNoStoreHeaders(httpContext.Response);
                var isControlPlaneHost = controlPlaneHostOptions.Value.IsControlPlaneHost(httpContext.Request.Host.Host);

                if (principal.Identity?.IsAuthenticated != true)
                {
                    return Results.Ok(new AuthStateResponse(false, null, Array.Empty<string>(), null, null, isControlPlaneHost));
                }

                var user = await userManager.GetUserAsync(principal);
                var roles = user is null
                    ? Array.Empty<string>()
                    : await userManager.GetRolesAsync(user);
                var tenantIdentifier = tenantContextAccessor.TenantContext?.Identifier;
                string? tenantRole = null;

                if (user is not null && !string.IsNullOrWhiteSpace(tenantIdentifier))
                {
                    tenantRole = await identityDbContext.UserTenantMemberships
                        .AsNoTracking()
                        .Where(membership => membership.UserId == user.Id
                            && membership.TenantIdentifier == tenantIdentifier
                            && membership.IsActive)
                        .Select(membership => membership.Role)
                        .SingleOrDefaultAsync();
                }

                return Results.Ok(
                    new AuthStateResponse(
                        true,
                        user?.UserName ?? principal.Identity?.Name,
                        roles.ToArray(),
                        tenantIdentifier,
                        tenantRole,
                        isControlPlaneHost));
            });

        return endpoints;
    }

    private static void ApplyNoStoreHeaders(HttpResponse response)
    {
        response.Headers.CacheControl = "no-store, no-cache, max-age=0";
        response.Headers.Pragma = "no-cache";
        response.Headers.Expires = "0";
    }
}
