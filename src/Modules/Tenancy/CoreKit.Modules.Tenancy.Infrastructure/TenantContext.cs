namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantContext
{
    public string Identifier { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Host { get; init; } = string.Empty;

    public string ConnectionString { get; init; } = string.Empty;
}
