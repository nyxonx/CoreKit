using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Identity.Application;

[RpcOperation("identity.memberships.list")]
public sealed record GetTenantMembershipsQuery : IQuery<IReadOnlyList<TenantMembershipDto>>;
