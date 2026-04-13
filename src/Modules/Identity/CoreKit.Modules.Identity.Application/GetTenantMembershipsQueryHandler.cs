using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Identity.Application;

public sealed class GetTenantMembershipsQueryHandler(
    ICurrentExecutionContextAccessor executionContextAccessor,
    ICurrentTenantAuthorizationService tenantAuthorizationService,
    ITenantMembershipAdministrationService tenantMembershipAdministrationService)
    : IRequestHandler<GetTenantMembershipsQuery, OperationResult<IReadOnlyList<TenantMembershipDto>>>
{
    public async Task<OperationResult<IReadOnlyList<TenantMembershipDto>>> Handle(
        GetTenantMembershipsQuery request,
        CancellationToken cancellationToken)
    {
        var authorizationError = await tenantAuthorizationService.ValidateAccessAsync(
            new CurrentTenantAccessRequirement(TenantMembershipRoles.Admin),
            cancellationToken);

        if (authorizationError is not null)
        {
            return OperationResult<IReadOnlyList<TenantMembershipDto>>.Invalid([authorizationError]);
        }

        var tenantIdentifier = executionContextAccessor.GetCurrent().TenantIdentifier;

        if (string.IsNullOrWhiteSpace(tenantIdentifier))
        {
            return OperationResult<IReadOnlyList<TenantMembershipDto>>.Failure(
                "tenant_context_required",
                "A tenant context is required for this operation.");
        }

        var memberships = await tenantMembershipAdministrationService.GetMembershipsAsync(
            tenantIdentifier,
            cancellationToken);

        return OperationResult<IReadOnlyList<TenantMembershipDto>>.Success(memberships);
    }
}
