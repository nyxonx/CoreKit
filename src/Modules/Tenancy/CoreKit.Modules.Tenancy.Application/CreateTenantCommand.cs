using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Tenancy.Application;

[RpcOperation("tenancy.catalog.create")]
public sealed record CreateTenantCommand(string Identifier, string Name, string Host) : ICommand<TenantCatalogDto>;
