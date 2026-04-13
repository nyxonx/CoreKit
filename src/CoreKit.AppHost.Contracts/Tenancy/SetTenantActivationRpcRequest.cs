namespace CoreKit.AppHost.Contracts.Tenancy;

public sealed record SetTenantActivationRpcRequest(
    string TenantIdentifier,
    bool IsActive);
