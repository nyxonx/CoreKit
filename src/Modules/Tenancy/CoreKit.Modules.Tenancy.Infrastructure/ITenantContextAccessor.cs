namespace CoreKit.Modules.Tenancy.Infrastructure;

public interface ITenantContextAccessor
{
    TenantContext? TenantContext { get; set; }
}
