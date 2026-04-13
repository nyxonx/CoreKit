using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Customers.Application;

[RpcOperation("customers.list")]
public sealed record GetCustomersQuery : IQuery<IReadOnlyList<CustomerDto>>;
