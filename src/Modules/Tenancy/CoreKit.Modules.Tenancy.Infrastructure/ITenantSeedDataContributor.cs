namespace CoreKit.Modules.Tenancy.Infrastructure;

public interface ITenantSeedDataContributor
{
    string Name { get; }

    Task SeedAsync(TenantProvisioningContext context, CancellationToken cancellationToken = default);
}
