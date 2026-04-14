namespace CoreKit.AppHost.Contracts.Tenancy;

public sealed record CreateTenantRpcRequest(
    string Identifier,
    string Name,
    string Host);
