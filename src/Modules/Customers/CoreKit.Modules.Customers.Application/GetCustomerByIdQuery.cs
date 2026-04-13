using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Customers.Application;

[RpcOperation("customers.get")]
public sealed record GetCustomerByIdQuery(Guid Id) : IQuery<CustomerDto>;
