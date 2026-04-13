using CoreKit.AppHost.Contracts.Tenancy;

namespace CoreKit.AppHost.Client.Services;

public interface ITenantAdministrationClient
{
    Task<RpcInvocationResult<IReadOnlyList<TenantCatalogDto>>> GetTenantsAsync(
        CancellationToken cancellationToken = default);

    Task<RpcInvocationResult<TenantCatalogDto>> CreateTenantAsync(
        CreateTenantRpcRequest request,
        CancellationToken cancellationToken = default);

    Task<RpcInvocationResult<TenantCatalogDto>> SetTenantActivationAsync(
        SetTenantActivationRpcRequest request,
        CancellationToken cancellationToken = default);
}
