using CoreKit.AppHost.Contracts.Customers;

namespace CoreKit.AppHost.Client.Services;

public sealed class CustomersModuleClient(RpcClient rpcClient)
    : RpcModuleClientBase(rpcClient), ICustomersModuleClient
{
    public Task<RpcInvocationResult<CustomerDto>> GetCustomerAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        InvokeAsync<CustomerDto>(
            CustomersRpcOperations.Get,
            new { id },
            cancellationToken);

    public Task<RpcInvocationResult<IReadOnlyList<CustomerDto>>> GetCustomersAsync(
        CancellationToken cancellationToken = default) =>
        InvokeAsync<IReadOnlyList<CustomerDto>>(
            CustomersRpcOperations.List,
            cancellationToken: cancellationToken);

    public Task<RpcInvocationResult<CustomerDto>> CreateCustomerAsync(
        CreateCustomerRpcRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return InvokeAsync<CustomerDto>(
            CustomersRpcOperations.Create,
            request,
            cancellationToken);
    }

    public Task<RpcInvocationResult<CustomerDto>> UpdateCustomerAsync(
        UpdateCustomerRpcRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return InvokeAsync<CustomerDto>(
            CustomersRpcOperations.Update,
            request,
            cancellationToken);
    }
}
