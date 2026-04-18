namespace CoreKit.Modules.Tenancy.Domain;

public sealed record TenantResolutionResult(
    bool IsResolved,
    string? FailureReason,
    TenantRuntimeInfo? Tenant)
{
    public static TenantResolutionResult Success(TenantRuntimeInfo tenant) =>
        new(true, null, tenant);

    public static TenantResolutionResult Failure(string failureReason) =>
        new(false, failureReason, null);
}
