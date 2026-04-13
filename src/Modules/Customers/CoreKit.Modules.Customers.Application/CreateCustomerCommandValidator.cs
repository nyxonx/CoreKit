using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Customers.Application;

public sealed class CreateCustomerCommandValidator : IValidator<CreateCustomerCommand>
{
    public Task<IReadOnlyList<OperationError>> ValidateAsync(
        CreateCustomerCommand request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var errors = new List<OperationError>();

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
