using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Tenancy.Application;

public sealed class GetTenantsQueryHandler(
    ICurrentExecutionContextAccessor executionContextAccessor,
    ITenantAdministrationService tenantAdministrationService)
    : IRequestHandler<GetTenantsQuery, OperationResult<IReadOnlyList<TenantCatalogDto>>>
{
    public async Task<OperationResult<IReadOnlyList<TenantCatalogDto>>> Handle(
        GetTenantsQuery request,
        CancellationToken cancellationToken)
    {
        if (!HasGlobalAdminRole(executionContextAccessor.GetCurrent()))
        {
            return OperationResult<IReadOnlyList<TenantCatalogDto>>.Failure(
                "global_admin_required",
                "Global admin access is required for this operation.");
        }

        var tenants = await tenantAdministrationService.GetTenantsAsync(cancellationToken);
        return OperationResult<IReadOnlyList<TenantCatalogDto>>.Success(tenants);
    }

    private static bool HasGlobalAdminRole(CurrentExecutionContext current) =>
        current.Roles?.Contains(ApplicationRoles.Admin, StringComparer.OrdinalIgnoreCase) == true;
}
