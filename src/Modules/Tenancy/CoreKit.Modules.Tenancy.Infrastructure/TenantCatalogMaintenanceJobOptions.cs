namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantCatalogMaintenanceJobOptions
{
    public const string SectionName = "BackgroundJobs:TenantCatalogMaintenance";

    public int IntervalMinutes { get; set; } = 5;
}
