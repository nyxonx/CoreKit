using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Customers.Application;

public sealed class DeleteCustomerCommandHandler(
    ICustomerService customerService,
    ICurrentTenantAuthorizationService tenantAuthorizationService)
    : IRequestHandler<DeleteCustomerCommand, OperationResult<bool>>
{
    public async Task<OperationResult<bool>> Handle(
        DeleteCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var authorizationError = await tenantAuthorizationService.ValidateAccessAsync(cancellationToken);

        if (authorizationError is not null)
        {
            return OperationResult<bool>.Invalid([authorizationError]);
        }

        var deleted = await customerService.DeleteCustomerAsync(request.Id, cancellationToken);

        return deleted
            ? OperationResult<bool>.Success(true)
            : OperationResult<bool>.Failure("customer_not_found", "Customer was not found.");
    }
}
