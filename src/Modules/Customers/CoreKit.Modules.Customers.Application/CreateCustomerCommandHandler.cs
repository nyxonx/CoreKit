using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Customers.Application;

public sealed class CreateCustomerCommandHandler(ICustomerService customerService)
    : IRequestHandler<CreateCustomerCommand, OperationResult<CustomerDto>>
{
    public async Task<OperationResult<CustomerDto>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await customerService.CreateCustomerAsync(
            new CreateCustomerRequest(request.Name, request.Email),
            cancellationToken);

        return OperationResult<CustomerDto>.Success(customer);
    }
}
