using System.Security.Claims;
using CoreKit.AppHost.Contracts.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace CoreKit.PlatformAppHost.Client.Services;

public sealed class ServerAuthenticationStateProvider(AuthApiClient authApiClient) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authState = await authApiClient.GetAuthStateAsync();
        return CreateAuthenticationState(authState);
    }

    public async Task RefreshUserStateAsync()
    {
        var authState = await authApiClient.GetAuthStateAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(CreateAuthenticationState(authState)));
    }

    private static AuthenticationState CreateAuthenticationState(PlatformAuthStateResponse response)
    {
        if (!response.IsAuthenticated)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = new List<Claim>();

        if (!string.IsNullOrWhiteSpace(response.UserName))
        {
            claims.Add(new Claim(ClaimTypes.Name, response.UserName));
        }

        foreach (var role in response.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "server")));
    }
}
