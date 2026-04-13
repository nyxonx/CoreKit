using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Customers.Application;

public sealed class GetCustomerByIdQueryHandler(ICustomerService customerService)
    : IRequestHandler<GetCustomerByIdQuery, OperationResult<CustomerDto>>
{
    public async Task<OperationResult<CustomerDto>> Handle(
        GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await customerService.GetCustomerAsync(request.Id, cancellationToken);

        return customer is null
            ? OperationResult<CustomerDto>.Failure("customer_not_found", "Customer was not found.")
            : OperationResult<CustomerDto>.Success(customer);
    }
}
