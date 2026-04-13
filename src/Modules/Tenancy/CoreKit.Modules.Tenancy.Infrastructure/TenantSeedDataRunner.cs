namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantSeedDataRunner(IEnumerable<ITenantSeedDataContributor> contributors)
{
    public async Task SeedAsync(
        TenantProvisioningContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        foreach (var contributor in contributors.OrderBy(item => item.Name, StringComparer.Ordinal))
        {
            await contributor.SeedAsync(context, cancellationToken);
        }
    }
}
