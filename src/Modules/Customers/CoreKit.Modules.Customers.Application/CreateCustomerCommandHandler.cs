using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Customers.Application;

public sealed class CreateCustomerCommandHandler(
    ICustomerService customerService,
    ICurrentTenantAuthorizationService tenantAuthorizationService)
    : IRequestHandler<CreateCustomerCommand, OperationResult<CustomerDto>>
{
    public async Task<OperationResult<CustomerDto>> Handle(
        CreateCustomerCommand request,
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
            var customer = await customerService.CreateCustomerAsync(
                new CreateCustomerRequest(request.Name, request.Email),
                cancellationToken);

            return OperationResult<CustomerDto>.Success(customer);
        }
        catch (InvalidOperationException exception)
        {
            return OperationResult<CustomerDto>.Failure("customer_email_conflict", exception.Message);
        }
    }
}
