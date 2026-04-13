namespace CoreKit.Modules.Tenancy.Application;

public sealed record CreateTenantRequest(
    string Identifier,
    string Name,
    string Host);
