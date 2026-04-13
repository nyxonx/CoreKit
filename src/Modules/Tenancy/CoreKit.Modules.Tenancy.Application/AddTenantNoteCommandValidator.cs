using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Tenancy.Application;

public sealed class AddTenantNoteCommandValidator : IValidator<AddTenantNoteCommand>
{
    public Task<IReadOnlyList<OperationError>> ValidateAsync(
        AddTenantNoteCommand request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!string.IsNullOrWhiteSpace(request.Value))
        {
            return Task.FromResult<IReadOnlyList<OperationError>>(Array.Empty<OperationError>());
        }

        return Task.FromResult<IReadOnlyList<OperationError>>(
        [
            new OperationError("validation_error", "Tenant note value is required.")
        ]);
    }
}
