using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Identity.Application;

[RpcOperation("identity.memberships.upsert")]
public sealed record UpsertTenantMembershipCommand(string UserName, string Role) : ICommand<TenantMembershipDto>;
