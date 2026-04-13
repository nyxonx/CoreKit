using CoreKit.AppHost.Contracts.Rpc;

namespace CoreKit.AppHost.Client.Services;

public sealed record RpcInvocationResult<TResponse>(
    bool Succeeded,
    TResponse? Data,
    IReadOnlyList<RpcErrorResponse> Errors);
