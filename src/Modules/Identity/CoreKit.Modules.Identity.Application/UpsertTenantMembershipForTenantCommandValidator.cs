using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Identity.Application;

public sealed class UpsertTenantMembershipForTenantCommandValidator : IValidator<UpsertTenantMembershipForTenantCommand>
{
    public Task<IReadOnlyList<OperationError>> ValidateAsync(
        UpsertTenantMembershipForTenantCommand instance,
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

        if (string.IsNullOrWhiteSpace(instance.Role))
        {
            errors.Add(new OperationError("validation_error", "Role is required."));
        }
        else if (instance.Role is not TenantMembershipRoles.Admin and not TenantMembershipRoles.Member)
        {
            errors.Add(
                new OperationError(
                    "validation_error",
                    $"Role must be '{TenantMembershipRoles.Admin}' or '{TenantMembershipRoles.Member}'."));
        }

        return Task.FromResult<IReadOnlyList<OperationError>>(errors);
    }
}
