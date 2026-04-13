namespace CoreKit.Modules.Tenancy.Application;

public interface ITenantAdministrationService
{
    Task<IReadOnlyList<TenantCatalogDto>> GetTenantsAsync(CancellationToken cancellationToken = default);

    Task<TenantCatalogDto> CreateTenantAsync(
        CreateTenantRequest request,
        CancellationToken cancellationToken = default);
}
