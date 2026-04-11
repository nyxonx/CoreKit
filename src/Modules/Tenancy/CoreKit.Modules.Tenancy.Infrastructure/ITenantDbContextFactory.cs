namespace CoreKit.Modules.Tenancy.Infrastructure;

public interface ITenantDbContextFactory
{
    TenantAppDbContext CreateDbContext();
}
