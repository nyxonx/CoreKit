using CoreKit.AppHost.Contracts.Customers;

namespace CoreKit.AppHost.Client.Services;

public interface ICustomersModuleClient
{
    Task<RpcInvocationResult<IReadOnlyList<CustomerDto>>> GetCustomersAsync(
        CancellationToken cancellationToken = default);

    Task<RpcInvocationResult<CustomerDto>> CreateCustomerAsync(
        CreateCustomerRpcRequest request,
        CancellationToken cancellationToken = default);
}
