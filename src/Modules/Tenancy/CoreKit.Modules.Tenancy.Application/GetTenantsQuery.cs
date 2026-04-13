using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Tenancy.Application;

[RpcOperation("tenancy.catalog.list")]
public sealed record GetTenantsQuery : IQuery<IReadOnlyList<TenantCatalogDto>>;
