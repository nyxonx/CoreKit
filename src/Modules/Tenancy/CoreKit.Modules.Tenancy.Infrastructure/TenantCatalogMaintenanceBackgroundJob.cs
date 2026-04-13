using CoreKit.BuildingBlocks.Presentation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantCatalogMaintenanceBackgroundJob(
    IServiceScopeFactory scopeFactory,
    IOptions<TenantCatalogMaintenanceJobOptions> options,
    IAuditEventWriter auditEventWriter,
    ILogger<TenantCatalogMaintenanceBackgroundJob> logger) : ICoreKitBackgroundJob
{
    public string Name => "tenancy.catalog-maintenance";

    public TimeSpan Interval => TimeSpan.FromMinutes(options.Value.IntervalMinutes);

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var validator = scope.ServiceProvider.GetRequiredService<TenantConfigurationValidator>();
        var provisioningService = scope.ServiceProvider.GetRequiredService<TenantProvisioningService>();

        await validator.ValidateAsync(cancellationToken);
        await provisioningService.ProvisionAllAsync(cancellationToken);

        logger.LogInformation("Tenant catalog maintenance completed successfully.");

        await auditEventWriter.WriteAsync(
            new AuditEvent(
                "background-job",
                Name,
                "success"),
            cancellationToken);
    }
}
