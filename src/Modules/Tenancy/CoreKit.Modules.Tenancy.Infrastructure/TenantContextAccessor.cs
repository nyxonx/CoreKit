namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantContextAccessor : ITenantContextAccessor
{
    public TenantContext? TenantContext { get; set; }
}
