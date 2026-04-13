using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Tenancy.Application;

public sealed class CreateTenantCommandHandler(
    ICurrentExecutionContextAccessor executionContextAccessor,
    ITenantAdministrationService tenantAdministrationService)
    : IRequestHandler<CreateTenantCommand, OperationResult<TenantCatalogDto>>
{
    public async Task<OperationResult<TenantCatalogDto>> Handle(
        CreateTenantCommand request,
        CancellationToken cancellationToken)
    {
        if (!HasGlobalAdminRole(executionContextAccessor.GetCurrent()))
        {
            return OperationResult<TenantCatalogDto>.Failure(
                "global_admin_required",
                "Global admin access is required for this operation.");
        }

        try
        {
            var tenant = await tenantAdministrationService.CreateTenantAsync(
                new CreateTenantRequest(request.Identifier, request.Name, request.Host),
                cancellationToken);

            return OperationResult<TenantCatalogDto>.Success(tenant);
        }
        catch (InvalidOperationException exception)
        {
            return OperationResult<TenantCatalogDto>.Failure("tenant_conflict", exception.Message);
        }
    }

    private static bool HasGlobalAdminRole(CurrentExecutionContext current) =>
        current.Roles?.Contains(ApplicationRoles.Admin, StringComparer.OrdinalIgnoreCase) == true;
}
