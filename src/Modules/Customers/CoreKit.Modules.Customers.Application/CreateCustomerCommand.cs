using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Customers.Application;

[RpcOperation("customers.create")]
public sealed record CreateCustomerCommand(string Name, string? Email) : ICommand<CustomerDto>;
