using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using CoreKit.AppHost.Contracts.Authentication;
using CoreKit.Modules.Identity.Domain;
using System.Security.Claims;

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
                SignInManager<AppUser> signInManager) =>
            {
                var result = await signInManager.PasswordSignInAsync(
                    request.UserName,
                    request.Password,
                    request.RememberMe,
                    lockoutOnFailure: false);

                return result.Succeeded
                    ? Results.Ok()
                    : Results.Unauthorized();
            });

        group.MapPost(
            "/logout",
            async (SignInManager<AppUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            });

        group.MapGet(
            "/state",
            async (
                ClaimsPrincipal principal,
                UserManager<AppUser> userManager) =>
            {
                if (principal.Identity?.IsAuthenticated != true)
                {
                    return Results.Ok(new AuthStateResponse(false, null, Array.Empty<string>()));
                }

                var user = await userManager.GetUserAsync(principal);
                var roles = user is null
                    ? Array.Empty<string>()
                    : await userManager.GetRolesAsync(user);

                return Results.Ok(
                    new AuthStateResponse(
                        true,
                        user?.UserName ?? principal.Identity?.Name,
                        roles.ToArray()));
            });

        return endpoints;
    }
}
