using CoreKit.AppHost.Contracts.Authentication;
using System.Net;
using System.Net.Http.Json;

namespace CoreKit.AppHost.Client.Services;

public sealed class AuthApiClient(HttpClient httpClient)
{
    public async Task<AuthStateResponse> GetAuthStateAsync()
    {
        var response =
            await httpClient.GetFromJsonAsync<AuthStateResponse>("/api/auth/state");

        return response ?? new AuthStateResponse(false, null, Array.Empty<string>());
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/api/auth/login", request);
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await httpClient.PostAsync("/api/auth/logout", content: null);
    }
}
