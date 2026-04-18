namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantRegistryOptions
{
    public const string SectionName = "Tenancy:Registry";

    public string Mode { get; set; } = "Local";

    public string? BaseUrl { get; set; }

    public bool IsRemoteMode() =>
        string.Equals(Mode, "Remote", StringComparison.OrdinalIgnoreCase);
}
