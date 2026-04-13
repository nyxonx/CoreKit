using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Customers.Application;

[RpcOperation("customers.delete")]
public sealed record DeleteCustomerCommand(Guid Id) : ICommand<bool>;
