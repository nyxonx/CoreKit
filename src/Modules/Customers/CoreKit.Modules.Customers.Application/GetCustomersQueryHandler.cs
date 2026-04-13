using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Customers.Application;

public sealed class GetCustomersQueryHandler(
    ICustomerService customerService,
    ICurrentTenantAuthorizationService tenantAuthorizationService)
    : IRequestHandler<GetCustomersQuery, OperationResult<IReadOnlyList<CustomerDto>>>
{
    public async Task<OperationResult<IReadOnlyList<CustomerDto>>> Handle(
        GetCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var authorizationError = await tenantAuthorizationService.ValidateAccessAsync(
            new CurrentTenantAccessRequirement(TenantMembershipRoles.Admin, TenantMembershipRoles.Member),
            cancellationToken);

        if (authorizationError is not null)
        {
            return OperationResult<IReadOnlyList<CustomerDto>>.Invalid([authorizationError]);
        }

        var customers = await customerService.GetCustomersAsync(cancellationToken);
        return OperationResult<IReadOnlyList<CustomerDto>>.Success(customers);
    }
}
