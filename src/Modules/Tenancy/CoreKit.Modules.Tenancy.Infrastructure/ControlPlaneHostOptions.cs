namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class ControlPlaneHostOptions
{
    public const string SectionName = "Tenancy:ControlPlaneHosts";

    public string[] Hosts { get; set; } = [];

    public bool IsControlPlaneHost(string? host)
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            return false;
        }

        return Hosts.Any(
            configuredHost => string.Equals(
                configuredHost?.Trim(),
                host.Trim(),
                StringComparison.OrdinalIgnoreCase));
    }
}
