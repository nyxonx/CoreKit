using System.Net.Http.Json;
using CoreKit.AppHost.Contracts.Authentication;

namespace CoreKit.PlatformAppHost.Client.Services;

public sealed class AuthApiClient(HttpClient httpClient)
{
    public async Task<AuthStateResponse> GetAuthStateAsync()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<AuthStateResponse>("/api/auth/state");
            return response ?? new AuthStateResponse(false, null, Array.Empty<string>(), null, null, true);
        }
        catch (HttpRequestException)
        {
            return new AuthStateResponse(false, null, Array.Empty<string>(), null, null, true);
        }
        catch (NotSupportedException)
        {
            return new AuthStateResponse(false, null, Array.Empty<string>(), null, null, true);
        }
        catch (TaskCanceledException)
        {
            return new AuthStateResponse(false, null, Array.Empty<string>(), null, null, true);
        }
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
