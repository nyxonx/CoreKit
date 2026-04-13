using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Identity.Application;

public sealed class SetTenantMembershipActivationForTenantCommandValidator : IValidator<SetTenantMembershipActivationForTenantCommand>
{
    public Task<IReadOnlyList<OperationError>> ValidateAsync(
        SetTenantMembershipActivationForTenantCommand instance,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var errors = new List<OperationError>();

        if (string.IsNullOrWhiteSpace(instance.TenantIdentifier))
        {
            errors.Add(new OperationError("validation_error", "Tenant identifier is required."));
        }

        if (string.IsNullOrWhiteSpace(instance.UserName))
        {
            errors.Add(new OperationError("validation_error", "User name is required."));
        }

        return Task.FromResult<IReadOnlyList<OperationError>>(errors);
    }
}
