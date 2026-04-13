using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Customers.Application;

public sealed class DeleteCustomerCommandValidator : IValidator<DeleteCustomerCommand>
{
    public Task<IReadOnlyList<OperationError>> ValidateAsync(
        DeleteCustomerCommand request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Id != Guid.Empty)
        {
            return Task.FromResult<IReadOnlyList<OperationError>>(Array.Empty<OperationError>());
        }

        return Task.FromResult<IReadOnlyList<OperationError>>(
        [
            new OperationError("validation_error", "Customer id is required.")
        ]);
    }
}
