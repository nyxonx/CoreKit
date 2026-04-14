namespace CoreKit.AppHost.Contracts.Identity;

public sealed record UpsertTenantMembershipRpcRequest(
    string UserName,
    string Role);
