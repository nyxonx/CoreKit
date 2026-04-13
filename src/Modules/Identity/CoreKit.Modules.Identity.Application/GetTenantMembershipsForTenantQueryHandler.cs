using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Identity.Application;

public sealed class GetTenantMembershipsForTenantQueryHandler(
    ICurrentExecutionContextAccessor executionContextAccessor,
    ITenantMembershipAdministrationService tenantMembershipAdministrationService)
    : IRequestHandler<GetTenantMembershipsForTenantQuery, OperationResult<IReadOnlyList<TenantMembershipDto>>>
{
    public async Task<OperationResult<IReadOnlyList<TenantMembershipDto>>> Handle(
        GetTenantMembershipsForTenantQuery request,
        CancellationToken cancellationToken)
    {
        if (!HasGlobalAdminRole(executionContextAccessor.GetCurrent()))
        {
            return OperationResult<IReadOnlyList<TenantMembershipDto>>.Failure(
                "global_admin_required",
                "Global admin access is required for this operation.");
        }

        if (string.IsNullOrWhiteSpace(request.TenantIdentifier))
        {
            return OperationResult<IReadOnlyList<TenantMembershipDto>>.Failure(
                "tenant_identifier_required",
                "A tenant identifier is required for this operation.");
        }

        var memberships = await tenantMembershipAdministrationService.GetMembershipsAsync(
            request.TenantIdentifier,
            cancellationToken);

        return OperationResult<IReadOnlyList<TenantMembershipDto>>.Success(memberships);
    }

    private static bool HasGlobalAdminRole(CurrentExecutionContext current) =>
        current.Roles?.Contains(ApplicationRoles.Admin, StringComparer.OrdinalIgnoreCase) == true;
}
