namespace CoreKit.Modules.Tenancy.Infrastructure;

public interface ITenantDatabaseMigration
{
    string Name { get; }

    Task ApplyAsync(TenantProvisioningContext context, CancellationToken cancellationToken = default);
}
