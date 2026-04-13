using CoreKit.Modules.Tenancy.Domain;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed record TenantProvisioningContext(TenantCatalogEntry Tenant);
