namespace CoreKit.PlatformAppHost.Client.Services;

public abstract class RpcModuleClientBase(RpcClient rpcClient)
{
    protected Task<RpcInvocationResult<TResponse>> InvokeAsync<TResponse>(
        string operation,
        object? payload = null,
        CancellationToken cancellationToken = default) =>
        rpcClient.InvokeAsync<TResponse>(operation, payload, cancellationToken);
}
