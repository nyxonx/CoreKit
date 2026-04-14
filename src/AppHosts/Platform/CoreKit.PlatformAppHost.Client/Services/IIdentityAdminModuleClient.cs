using CoreKit.AppHost.Contracts.Identity;

namespace CoreKit.PlatformAppHost.Client.Services;

public interface IIdentityAdminModuleClient
{
    Task<RpcInvocationResult<IReadOnlyList<TenantMembershipDto>>> GetTenantMembershipsForTenantAsync(
        GetTenantMembershipsForTenantRpcRequest request,
        CancellationToken cancellationToken = default);

    Task<RpcInvocationResult<TenantMembershipDto>> UpsertTenantMembershipForTenantAsync(
        UpsertTenantMembershipForTenantRpcRequest request,
        CancellationToken cancellationToken = default);

    Task<RpcInvocationResult<TenantMembershipDto>> SetTenantMembershipActivationForTenantAsync(
        SetTenantMembershipActivationForTenantRpcRequest request,
        CancellationToken cancellationToken = default);
}
