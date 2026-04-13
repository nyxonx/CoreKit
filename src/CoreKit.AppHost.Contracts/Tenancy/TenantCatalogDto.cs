namespace CoreKit.AppHost.Contracts.Tenancy;

public sealed record TenantCatalogDto(
    string Identifier,
    string Name,
    string Host,
    bool IsActive);
