using CoreKit.AppHost.Contracts.Identity;

namespace CoreKit.AppHost.Client.Services;

public sealed class IdentityAdminModuleClient(RpcClient rpcClient)
    : RpcModuleClientBase(rpcClient), IIdentityAdminModuleClient
{
    public Task<RpcInvocationResult<IReadOnlyList<TenantMembershipDto>>> GetTenantMembershipsAsync(
        CancellationToken cancellationToken = default) =>
        InvokeAsync<IReadOnlyList<TenantMembershipDto>>(
            IdentityRpcOperations.ListMemberships,
            cancellationToken: cancellationToken);

    public Task<RpcInvocationResult<TenantMembershipDto>> UpsertTenantMembershipAsync(
        UpsertTenantMembershipRpcRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return InvokeAsync<TenantMembershipDto>(
            IdentityRpcOperations.UpsertMembership,
            request,
            cancellationToken);
    }
}
