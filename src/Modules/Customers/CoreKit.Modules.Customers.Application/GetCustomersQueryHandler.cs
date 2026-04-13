using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Customers.Application;

public sealed class GetCustomersQueryHandler(ICustomerService customerService)
    : IRequestHandler<GetCustomersQuery, OperationResult<IReadOnlyList<CustomerDto>>>
{
    public async Task<OperationResult<IReadOnlyList<CustomerDto>>> Handle(
        GetCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await customerService.GetCustomersAsync(cancellationToken);
        return OperationResult<IReadOnlyList<CustomerDto>>.Success(customers);
    }
}
