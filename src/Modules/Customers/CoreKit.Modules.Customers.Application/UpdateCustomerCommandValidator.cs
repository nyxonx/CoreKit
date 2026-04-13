using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Customers.Application;

public sealed class UpdateCustomerCommandValidator : IValidator<UpdateCustomerCommand>
{
    public Task<IReadOnlyList<OperationError>> ValidateAsync(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var errors = new List<OperationError>();

        if (request.Id == Guid.Empty)
        {
            errors.Add(new OperationError("validation_error", "Customer id is required."));
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors.Add(new OperationError("validation_error", "Customer name is required."));
        }

        if (!string.IsNullOrWhiteSpace(request.Email)
            && !request.Email.Contains('@', StringComparison.Ordinal))
        {
            errors.Add(new OperationError("validation_error", "Customer email must be valid."));
        }

        return Task.FromResult<IReadOnlyList<OperationError>>(errors);
    }
}
