namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantConnectionStringProvider(ITenantContextAccessor tenantContextAccessor)
    : ITenantConnectionStringProvider
{
    public string GetRequiredConnectionString()
    {
        var tenantContext = tenantContextAccessor.TenantContext;

        if (tenantContext is null)
        {
            throw new InvalidOperationException("Tenant context is not available for the current request.");
        }

        if (string.IsNullOrWhiteSpace(tenantContext.ConnectionString))
        {
            throw new InvalidOperationException(
                $"Tenant '{tenantContext.Identifier}' does not have a configured connection string.");
        }

        return tenantContext.ConnectionString;
    }
}
