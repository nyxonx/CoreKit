using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Identity.Application;

[RpcOperation("identity.platform-memberships.list")]
public sealed record GetTenantMembershipsForTenantQuery(string TenantIdentifier)
    : IQuery<IReadOnlyList<TenantMembershipDto>>;
