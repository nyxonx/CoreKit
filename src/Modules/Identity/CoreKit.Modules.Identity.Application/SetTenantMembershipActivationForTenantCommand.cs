using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Identity.Application;

[RpcOperation("identity.platform-memberships.activation")]
public sealed record SetTenantMembershipActivationForTenantCommand(
    string TenantIdentifier,
    string UserName,
    bool IsActive) : ICommand<TenantMembershipDto>;
