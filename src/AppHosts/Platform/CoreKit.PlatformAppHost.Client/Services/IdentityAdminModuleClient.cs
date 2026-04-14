using CoreKit.AppHost.Contracts.Identity;

namespace CoreKit.PlatformAppHost.Client.Services;

public sealed class IdentityAdminModuleClient(RpcClient rpcClient)
    : RpcModuleClientBase(rpcClient), IIdentityAdminModuleClient
{
    public Task<RpcInvocationResult<IReadOnlyList<TenantMembershipDto>>> GetTenantMembershipsForTenantAsync(
        GetTenantMembershipsForTenantRpcRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return InvokeAsync<IReadOnlyList<TenantMembershipDto>>(
            IdentityRpcOperations.ListMembershipsForTenant,
            request,
            cancellationToken);
    }

    public Task<RpcInvocationResult<TenantMembershipDto>> UpsertTenantMembershipForTenantAsync(
        UpsertTenantMembershipForTenantRpcRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return InvokeAsync<TenantMembershipDto>(
            IdentityRpcOperations.UpsertMembershipForTenant,
            request,
            cancellationToken);
    }

    public Task<RpcInvocationResult<TenantMembershipDto>> SetTenantMembershipActivationForTenantAsync(
        SetTenantMembershipActivationForTenantRpcRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return InvokeAsync<TenantMembershipDto>(
            IdentityRpcOperations.SetMembershipActivationForTenant,
            request,
            cancellationToken);
    }
}
