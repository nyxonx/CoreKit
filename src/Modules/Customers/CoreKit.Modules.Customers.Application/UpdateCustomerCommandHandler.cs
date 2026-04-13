using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Customers.Application;

public sealed class UpdateCustomerCommandHandler(
    ICustomerService customerService,
    ICurrentTenantAuthorizationService tenantAuthorizationService)
    : IRequestHandler<UpdateCustomerCommand, OperationResult<CustomerDto>>
{
    public async Task<OperationResult<CustomerDto>> Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var authorizationError = await tenantAuthorizationService.ValidateAccessAsync(
            new CurrentTenantAccessRequirement(TenantMembershipRoles.Admin),
            cancellationToken);

        if (authorizationError is not null)
        {
            return OperationResult<CustomerDto>.Invalid([authorizationError]);
        }

        try
        {
            var customer = await customerService.UpdateCustomerAsync(
                new UpdateCustomerRequest(request.Id, request.Name, request.Email),
                cancellationToken);

            return customer is null
                ? OperationResult<CustomerDto>.Failure("customer_not_found", "Customer was not found.")
                : OperationResult<CustomerDto>.Success(customer);
        }
        catch (InvalidOperationException exception)
        {
            return OperationResult<CustomerDto>.Failure("customer_email_conflict", exception.Message);
        }
    }
}
