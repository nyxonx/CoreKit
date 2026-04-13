using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Identity.Application;

public sealed class UpsertTenantMembershipForTenantCommandHandler(
    ICurrentExecutionContextAccessor executionContextAccessor,
    ITenantMembershipAdministrationService tenantMembershipAdministrationService)
    : IRequestHandler<UpsertTenantMembershipForTenantCommand, OperationResult<TenantMembershipDto>>
{
    public async Task<OperationResult<TenantMembershipDto>> Handle(
        UpsertTenantMembershipForTenantCommand request,
        CancellationToken cancellationToken)
    {
        if (!HasGlobalAdminRole(executionContextAccessor.GetCurrent()))
        {
            return OperationResult<TenantMembershipDto>.Failure(
                "global_admin_required",
                "Global admin access is required for this operation.");
        }

        if (string.IsNullOrWhiteSpace(request.TenantIdentifier))
        {
            return OperationResult<TenantMembershipDto>.Failure(
                "tenant_identifier_required",
                "A tenant identifier is required for this operation.");
        }

        var membership = await tenantMembershipAdministrationService.UpsertMembershipAsync(
            request.TenantIdentifier,
            request.UserName,
            request.Role,
            cancellationToken);

        return membership is null
            ? OperationResult<TenantMembershipDto>.Failure(
                "identity_user_not_found",
                "The specified user was not found.")
            : OperationResult<TenantMembershipDto>.Success(membership);
    }

    private static bool HasGlobalAdminRole(CurrentExecutionContext current) =>
        current.Roles?.Contains(ApplicationRoles.Admin, StringComparer.OrdinalIgnoreCase) == true;
}
