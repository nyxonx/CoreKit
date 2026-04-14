using CoreKit.AppHost.Contracts.Rpc;

namespace CoreKit.PlatformAppHost.Client.Services;

public sealed record RpcInvocationResult<TResponse>(
    bool Succeeded,
    TResponse? Data,
    IReadOnlyList<RpcErrorResponse> Errors);
