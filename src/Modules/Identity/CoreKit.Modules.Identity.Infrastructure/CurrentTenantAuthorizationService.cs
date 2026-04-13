using CoreKit.BuildingBlocks.Application;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Identity.Infrastructure;

public sealed class CurrentTenantAuthorizationService(
    ICurrentExecutionContextAccessor executionContextAccessor,
    AppIdentityDbContext dbContext) : ICurrentTenantAuthorizationService
{
    public async Task<OperationError?> ValidateAccessAsync(CancellationToken cancellationToken = default)
    {
        var current = executionContextAccessor.GetCurrent();

        if (string.IsNullOrWhiteSpace(current.TenantIdentifier))
        {
            return new OperationError(
                "tenant_context_required",
                "A tenant context is required for this operation.");
        }

        if (!current.IsAuthenticated || string.IsNullOrWhiteSpace(current.UserId))
        {
            return new OperationError(
                "authentication_required",
                "Authentication is required for this operation.");
        }

        var hasMembership = await dbContext.UserTenantMemberships
            .AsNoTracking()
            .AnyAsync(
                membership => membership.UserId == current.UserId
                    && membership.TenantIdentifier == current.TenantIdentifier
                    && membership.IsActive,
                cancellationToken);

        return hasMembership
            ? null
            : new OperationError(
                "tenant_membership_required",
                "The current user does not have access to the active tenant.");
    }
}
