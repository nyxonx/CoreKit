namespace CoreKit.Modules.Tenancy.Domain;

public sealed record TenantResolutionResult(
    bool IsResolved,
    string? FailureReason,
    TenantCatalogEntry? Tenant)
{
    public static TenantResolutionResult Success(TenantCatalogEntry tenant) =>
        new(true, null, tenant);

    public static TenantResolutionResult Failure(string failureReason) =>
        new(false, failureReason, null);
}
