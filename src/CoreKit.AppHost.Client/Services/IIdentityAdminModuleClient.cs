using CoreKit.AppHost.Contracts.Identity;

namespace CoreKit.AppHost.Client.Services;

public interface IIdentityAdminModuleClient
{
    Task<RpcInvocationResult<IReadOnlyList<TenantMembershipDto>>> GetTenantMembershipsAsync(
        CancellationToken cancellationToken = default);

    Task<RpcInvocationResult<TenantMembershipDto>> UpsertTenantMembershipAsync(
        UpsertTenantMembershipRpcRequest request,
        CancellationToken cancellationToken = default);
}
