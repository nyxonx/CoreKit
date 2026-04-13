using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Identity.Application;

[RpcOperation("identity.platform-memberships.upsert")]
public sealed record UpsertTenantMembershipForTenantCommand(
    string TenantIdentifier,
    string UserName,
    string Role) : ICommand<TenantMembershipDto>;
