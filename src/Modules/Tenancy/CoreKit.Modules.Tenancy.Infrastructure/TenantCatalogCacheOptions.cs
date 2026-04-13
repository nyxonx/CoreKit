namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantCatalogCacheOptions
{
    public const string SectionName = "Caching:TenantCatalog";

    public int TtlSeconds { get; set; } = 60;
}
