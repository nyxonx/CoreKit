using System.Net;
using System.Net.Http.Json;
using CoreKit.AppHost.Contracts.Tenancy;
using CoreKit.Modules.Tenancy.Application;
using CoreKit.Modules.Tenancy.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class RemoteTenantRegistry(
    IHttpClientFactory httpClientFactory,
    IMemoryCache memoryCache,
    IOptions<TenantCatalogCacheOptions> cacheOptions,
    IOptions<TenantRegistryOptions> registryOptions) : ITenantRegistry
{
    public Task<TenantRuntimeInfo?> GetByIdentifierAsync(
        string identifier,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"tenant-registry:remote:identifier:{identifier}";
        var requestPath = $"/api/platform/tenant-registry/by-identifier/{Uri.EscapeDataString(identifier)}";

        return GetOrCreateAsync(cacheKey, requestPath, cancellationToken);
    }

    public Task<TenantRuntimeInfo?> GetByHostAsync(
        string host,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"tenant-registry:remote:host:{host}";
        var requestPath = $"/api/platform/tenant-registry/by-host/{Uri.EscapeDataString(host)}";

        return GetOrCreateAsync(cacheKey, requestPath, cancellationToken);
    }

    private async Task<TenantRuntimeInfo?> GetOrCreateAsync(
        string cacheKey,
        string requestPath,
        CancellationToken cancellationToken)
    {
        if (memoryCache.TryGetValue(cacheKey, out TenantRuntimeInfo? cachedTenant))
        {
            return cachedTenant;
        }

        var response = await SendAsync(requestPath, cancellationToken);
        if (response is null)
        {
            return null;
        }

        memoryCache.Set(
            cacheKey,
            response,
            TimeSpan.FromSeconds(cacheOptions.Value.TtlSeconds));

        return response;
    }

    private async Task<TenantRuntimeInfo?> SendAsync(
        string requestPath,
        CancellationToken cancellationToken)
    {
        var baseUrl = registryOptions.Value.BaseUrl;
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException(
                "Tenant registry is configured for remote mode, but no BaseUrl was provided.");
        }

        var client = httpClientFactory.CreateClient(nameof(RemoteTenantRegistry));
        client.BaseAddress = new Uri(baseUrl, UriKind.Absolute);

        using var response = await client.GetAsync(requestPath, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<TenantRegistryItemResponse>(cancellationToken);
        if (payload is null)
        {
            throw new InvalidOperationException("Remote tenant registry returned an empty payload.");
        }

        return new TenantRuntimeInfo(
            payload.Identifier,
            payload.Name,
            payload.Host,
            payload.IsActive,
            payload.ConnectionString);
    }
}
