namespace CoreKit.Modules.Tenancy.Domain;

public sealed class TenantCatalogEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Identifier { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string ConnectionString { get; set; } = string.Empty;
}
