namespace CoreKit.AppHost.Contracts.Tenancy;

public sealed record TenantRegistryItemResponse(
    string Identifier,
    string Name,
    string Host,
    bool IsActive,
    string ConnectionString);
