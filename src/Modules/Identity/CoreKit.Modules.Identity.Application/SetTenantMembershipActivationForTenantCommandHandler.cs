using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Identity.Application;

public sealed class SetTenantMembershipActivationForTenantCommandHandler(
    ICurrentExecutionContextAccessor executionContextAccessor,
    ITenantMembershipAdministrationService tenantMembershipAdministrationService)
    : IRequestHandler<SetTenantMembershipActivationForTenantCommand, OperationResult<TenantMembershipDto>>
{
    public async Task<OperationResult<TenantMembershipDto>> Handle(
        SetTenantMembershipActivationForTenantCommand request,
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

        var membership = await tenantMembershipAdministrationService.SetMembershipActivationAsync(
            request.TenantIdentifier,
            request.UserName,
            request.IsActive,
            cancellationToken);

        return membership is null
            ? OperationResult<TenantMembershipDto>.Failure(
                "tenant_membership_not_found",
                "The specified tenant membership was not found.")
            : OperationResult<TenantMembershipDto>.Success(membership);
    }

    private static bool HasGlobalAdminRole(CurrentExecutionContext current) =>
        current.Roles?.Contains(ApplicationRoles.Admin, StringComparer.OrdinalIgnoreCase) == true;
}
