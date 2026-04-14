namespace CoreKit.AppHost.Contracts.Identity;

public sealed record UpsertTenantMembershipForTenantRpcRequest(
    string TenantIdentifier,
    string UserName,
    string Role);
