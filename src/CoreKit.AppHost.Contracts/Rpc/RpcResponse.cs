namespace CoreKit.AppHost.Contracts.Rpc;

public sealed record RpcResponse(
    bool Succeeded,
    object? Data,
    IReadOnlyList<RpcErrorResponse> Errors);
