namespace CoreKit.AppHost.Contracts.Identity;

public sealed record SetTenantMembershipActivationForTenantRpcRequest(
    string TenantIdentifier,
    string UserName,
    bool IsActive);
