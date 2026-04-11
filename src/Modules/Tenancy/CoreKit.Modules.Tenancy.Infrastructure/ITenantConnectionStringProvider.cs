namespace CoreKit.Modules.Tenancy.Infrastructure;

public interface ITenantConnectionStringProvider
{
    string GetRequiredConnectionString();
}
