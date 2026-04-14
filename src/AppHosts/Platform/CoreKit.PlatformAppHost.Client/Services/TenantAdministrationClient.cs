using CoreKit.AppHost.Contracts.Tenancy;

namespace CoreKit.PlatformAppHost.Client.Services;

public sealed class TenantAdministrationClient(RpcClient rpcClient)
    : RpcModuleClientBase(rpcClient), ITenantAdministrationClient
{
    public Task<RpcInvocationResult<IReadOnlyList<TenantCatalogDto>>> GetTenantsAsync(
        CancellationToken cancellationToken = default) =>
        InvokeAsync<IReadOnlyList<TenantCatalogDto>>(
            TenancyRpcOperations.GetTenants,
            cancellationToken: cancellationToken);

    public Task<RpcInvocationResult<TenantCatalogDto>> CreateTenantAsync(
        CreateTenantRpcRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return InvokeAsync<TenantCatalogDto>(
            TenancyRpcOperations.CreateTenant,
            request,
            cancellationToken);
    }

    public Task<RpcInvocationResult<TenantCatalogDto>> SetTenantActivationAsync(
        SetTenantActivationRpcRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return InvokeAsync<TenantCatalogDto>(
            TenancyRpcOperations.SetTenantActivation,
            request,
            cancellationToken);
    }
}
