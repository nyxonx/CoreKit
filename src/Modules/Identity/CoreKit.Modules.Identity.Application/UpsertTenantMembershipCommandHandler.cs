using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Identity.Application;

public sealed class UpsertTenantMembershipCommandHandler(
    ICurrentExecutionContextAccessor executionContextAccessor,
    ICurrentTenantAuthorizationService tenantAuthorizationService,
    ITenantMembershipAdministrationService tenantMembershipAdministrationService)
    : IRequestHandler<UpsertTenantMembershipCommand, OperationResult<TenantMembershipDto>>
{
    public async Task<OperationResult<TenantMembershipDto>> Handle(
        UpsertTenantMembershipCommand request,
        CancellationToken cancellationToken)
    {
        var authorizationError = await tenantAuthorizationService.ValidateAccessAsync(
            new CurrentTenantAccessRequirement(TenantMembershipRoles.Admin),
            cancellationToken);

        if (authorizationError is not null)
        {
            return OperationResult<TenantMembershipDto>.Invalid([authorizationError]);
        }

        var tenantIdentifier = executionContextAccessor.GetCurrent().TenantIdentifier;

        if (string.IsNullOrWhiteSpace(tenantIdentifier))
        {
            return OperationResult<TenantMembershipDto>.Failure(
                "tenant_context_required",
                "A tenant context is required for this operation.");
        }

        var membership = await tenantMembershipAdministrationService.UpsertMembershipAsync(
            tenantIdentifier,
            request.UserName,
            request.Role,
            cancellationToken);

        return membership is null
            ? OperationResult<TenantMembershipDto>.Failure(
                "identity_user_not_found",
                "The specified user was not found.")
            : OperationResult<TenantMembershipDto>.Success(membership);
    }
}
