using System.Net.Http.Json;
using CoreKit.AppHost.Contracts.Authentication;

namespace CoreKit.PlatformAppHost.Client.Services;

public sealed class AuthApiClient(HttpClient httpClient)
{
    public async Task<PlatformAuthStateResponse> GetAuthStateAsync()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<PlatformAuthStateResponse>("/api/auth/state");
            return response ?? new PlatformAuthStateResponse(false, null, Array.Empty<string>());
        }
        catch (HttpRequestException)
        {
            return new PlatformAuthStateResponse(false, null, Array.Empty<string>());
        }
        catch (NotSupportedException)
        {
            return new PlatformAuthStateResponse(false, null, Array.Empty<string>());
        }
        catch (TaskCanceledException)
        {
            return new PlatformAuthStateResponse(false, null, Array.Empty<string>());
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
