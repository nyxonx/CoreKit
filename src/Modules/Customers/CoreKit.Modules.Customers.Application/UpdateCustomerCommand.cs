using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Customers.Application;

[RpcOperation("customers.update")]
public sealed record UpdateCustomerCommand(Guid Id, string Name, string? Email) : ICommand<CustomerDto>;
