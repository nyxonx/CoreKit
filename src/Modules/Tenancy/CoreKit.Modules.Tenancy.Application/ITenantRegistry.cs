using CoreKit.Modules.Tenancy.Domain;

namespace CoreKit.Modules.Tenancy.Application;

public interface ITenantRegistry
{
    Task<TenantRuntimeInfo?> GetByIdentifierAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    Task<TenantRuntimeInfo?> GetByHostAsync(
        string host,
        CancellationToken cancellationToken = default);
}
