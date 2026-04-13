namespace CoreKit.Modules.Tenancy.Application;

public sealed record TenantCatalogDto(
    string Identifier,
    string Name,
    string Host,
    bool IsActive);
