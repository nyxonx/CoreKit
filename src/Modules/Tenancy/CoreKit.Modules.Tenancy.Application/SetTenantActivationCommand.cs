using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Tenancy.Application;

[RpcOperation("tenancy.catalog.activation")]
public sealed record SetTenantActivationCommand(
    string TenantIdentifier,
    bool IsActive) : ICommand<TenantCatalogDto>;
