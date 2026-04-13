using CoreKit.AppHost.Contracts.Customers;

namespace CoreKit.AppHost.Client.Services;

public interface ICustomersModuleClient
{
    Task<RpcInvocationResult<CustomerDto>> GetCustomerAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<RpcInvocationResult<IReadOnlyList<CustomerDto>>> GetCustomersAsync(
        CancellationToken cancellationToken = default);

    Task<RpcInvocationResult<CustomerDto>> CreateCustomerAsync(
        CreateCustomerRpcRequest request,
        CancellationToken cancellationToken = default);

    Task<RpcInvocationResult<CustomerDto>> UpdateCustomerAsync(
        UpdateCustomerRpcRequest request,
        CancellationToken cancellationToken = default);
}
