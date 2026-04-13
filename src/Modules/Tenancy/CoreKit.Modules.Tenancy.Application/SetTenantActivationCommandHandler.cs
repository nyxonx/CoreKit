using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Tenancy.Application;

public sealed class SetTenantActivationCommandHandler(
    ICurrentExecutionContextAccessor executionContextAccessor,
    ITenantAdministrationService tenantAdministrationService)
    : IRequestHandler<SetTenantActivationCommand, OperationResult<TenantCatalogDto>>
{
    public async Task<OperationResult<TenantCatalogDto>> Handle(
        SetTenantActivationCommand request,
        CancellationToken cancellationToken)
    {
        if (!HasGlobalAdminRole(executionContextAccessor.GetCurrent()))
        {
            return OperationResult<TenantCatalogDto>.Failure(
                "global_admin_required",
                "Global admin access is required for this operation.");
        }

        if (string.IsNullOrWhiteSpace(request.TenantIdentifier))
        {
            return OperationResult<TenantCatalogDto>.Failure(
                "tenant_identifier_required",
                "A tenant identifier is required for this operation.");
        }

        var tenant = await tenantAdministrationService.SetTenantActivationAsync(
            request.TenantIdentifier,
            request.IsActive,
            cancellationToken);

        return tenant is null
            ? OperationResult<TenantCatalogDto>.Failure(
                "tenant_not_found",
                "The specified tenant was not found.")
            : OperationResult<TenantCatalogDto>.Success(tenant);
    }

    private static bool HasGlobalAdminRole(CurrentExecutionContext current) =>
        current.Roles?.Contains(ApplicationRoles.Admin, StringComparer.OrdinalIgnoreCase) == true;
}
