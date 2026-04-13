using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Customers.Application;

public sealed class GetCustomerByIdQueryHandler(
    ICustomerService customerService,
    ICurrentTenantAuthorizationService tenantAuthorizationService)
    : IRequestHandler<GetCustomerByIdQuery, OperationResult<CustomerDto>>
{
    public async Task<OperationResult<CustomerDto>> Handle(
        GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var authorizationError = await tenantAuthorizationService.ValidateAccessAsync(
            new CurrentTenantAccessRequirement(TenantMembershipRoles.Admin, TenantMembershipRoles.Member),
            cancellationToken);

        if (authorizationError is not null)
        {
            return OperationResult<CustomerDto>.Invalid([authorizationError]);
        }

        var customer = await customerService.GetCustomerAsync(request.Id, cancellationToken);

        return customer is null
            ? OperationResult<CustomerDto>.Failure("customer_not_found", "Customer was not found.")
            : OperationResult<CustomerDto>.Success(customer);
    }
}
