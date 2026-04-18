namespace CoreKit.Modules.Tenancy.Domain;

public sealed record TenantRuntimeInfo(
    string Identifier,
    string Name,
    string Host,
    bool IsActive,
    string ConnectionString);
