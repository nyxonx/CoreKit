using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Customers.Application;

public sealed class DeleteCustomerCommandHandler(ICustomerService customerService)
    : IRequestHandler<DeleteCustomerCommand, OperationResult<bool>>
{
    public async Task<OperationResult<bool>> Handle(
        DeleteCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var deleted = await customerService.DeleteCustomerAsync(request.Id, cancellationToken);

        return deleted
            ? OperationResult<bool>.Success(true)
            : OperationResult<bool>.Failure("customer_not_found", "Customer was not found.");
    }
}
