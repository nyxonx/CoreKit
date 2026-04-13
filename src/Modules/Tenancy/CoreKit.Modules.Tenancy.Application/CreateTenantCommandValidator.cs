using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Tenancy.Application;

public sealed class CreateTenantCommandValidator : IValidator<CreateTenantCommand>
{
    public Task<IReadOnlyList<OperationError>> ValidateAsync(
        CreateTenantCommand instance,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var errors = new List<OperationError>();

        if (string.IsNullOrWhiteSpace(instance.Identifier))
        {
            errors.Add(new OperationError("validation_error", "Identifier is required."));
        }

        if (string.IsNullOrWhiteSpace(instance.Name))
        {
            errors.Add(new OperationError("validation_error", "Name is required."));
        }

        if (string.IsNullOrWhiteSpace(instance.Host))
        {
            errors.Add(new OperationError("validation_error", "Host is required."));
        }

        return Task.FromResult<IReadOnlyList<OperationError>>(errors);
    }
}
