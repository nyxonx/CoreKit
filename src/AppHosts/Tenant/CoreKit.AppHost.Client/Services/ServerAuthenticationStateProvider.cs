using CoreKit.AppHost.Contracts.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CoreKit.AppHost.Client.Services;

public sealed class ServerAuthenticationStateProvider(AuthApiClient authApiClient)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authState = await authApiClient.GetAuthStateAsync();
        return CreateAuthenticationState(authState);
    }

    public async Task RefreshUserStateAsync()
    {
        var authState = await authApiClient.GetAuthStateAsync();
        NotifyAuthenticationStateChanged(
            Task.FromResult(CreateAuthenticationState(authState)));
    }

    private static AuthenticationState CreateAuthenticationState(AuthStateResponse authState)
    {
        if (!authState.IsAuthenticated || string.IsNullOrWhiteSpace(authState.UserName))
        {
            return new AuthenticationState(CreateAnonymousPrincipal(authState));
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, authState.UserName)
        };

        claims.AddRange(authState.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        if (!string.IsNullOrWhiteSpace(authState.TenantIdentifier))
        {
            claims.Add(new Claim("corekit:tenant", authState.TenantIdentifier));
        }

        if (!string.IsNullOrWhiteSpace(authState.TenantRole))
        {
            claims.Add(new Claim("corekit:tenant-role", authState.TenantRole));
        }

        var identity = new ClaimsIdentity(claims, authenticationType: "Cookies");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private static ClaimsPrincipal CreateAnonymousPrincipal(AuthStateResponse authState) =>
        new(new ClaimsIdentity());
}
