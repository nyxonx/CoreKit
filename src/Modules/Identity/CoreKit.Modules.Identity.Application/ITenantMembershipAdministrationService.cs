namespace CoreKit.Modules.Identity.Application;

public interface ITenantMembershipAdministrationService
{
    Task<IReadOnlyList<TenantMembershipDto>> GetMembershipsAsync(
        string tenantIdentifier,
        CancellationToken cancellationToken = default);

    Task<TenantMembershipDto?> UpsertMembershipAsync(
        string tenantIdentifier,
        string userName,
        string role,
        CancellationToken cancellationToken = default);

    Task<TenantMembershipDto?> SetMembershipActivationAsync(
        string tenantIdentifier,
        string userName,
        bool isActive,
        CancellationToken cancellationToken = default);
}
