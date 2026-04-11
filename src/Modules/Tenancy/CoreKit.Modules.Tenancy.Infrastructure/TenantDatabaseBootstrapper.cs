namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantDatabaseBootstrapper(ITenantDbContextFactory tenantDbContextFactory)
{
    public async Task EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = tenantDbContextFactory.CreateDbContext();
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}
