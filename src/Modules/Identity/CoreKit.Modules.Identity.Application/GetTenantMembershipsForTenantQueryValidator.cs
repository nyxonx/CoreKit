using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Identity.Application;

public sealed class GetTenantMembershipsForTenantQueryValidator : IValidator<GetTenantMembershipsForTenantQuery>
{
    public Task<IReadOnlyList<OperationError>> ValidateAsync(
        GetTenantMembershipsForTenantQuery instance,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var errors = new List<OperationError>();

        if (string.IsNullOrWhiteSpace(instance.TenantIdentifier))
        {
            errors.Add(new OperationError("validation_error", "Tenant identifier is required."));
        }

        return Task.FromResult<IReadOnlyList<OperationError>>(errors);
    }
}
