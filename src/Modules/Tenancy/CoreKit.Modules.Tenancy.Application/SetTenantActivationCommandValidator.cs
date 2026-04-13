using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Tenancy.Application;

public sealed class SetTenantActivationCommandValidator : IValidator<SetTenantActivationCommand>
{
    public Task<IReadOnlyList<OperationError>> ValidateAsync(
        SetTenantActivationCommand instance,
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
