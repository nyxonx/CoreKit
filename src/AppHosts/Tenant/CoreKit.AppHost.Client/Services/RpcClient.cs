using System.Net.Http.Json;
using System.Text.Json;
using CoreKit.AppHost.Contracts.Rpc;

namespace CoreKit.AppHost.Client.Services;

public sealed class RpcClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<RpcInvocationResult<TResponse>> InvokeAsync<TResponse>(
        string operation,
        object? payload = null,
        CancellationToken cancellationToken = default)
    {
        var requestPayload = payload is null
            ? JsonSerializer.SerializeToElement(new { }, JsonOptions)
            : JsonSerializer.SerializeToElement(payload, JsonOptions);

        return InvokeCoreAsync<TResponse>(
            new RpcRequest(operation, requestPayload),
            cancellationToken);
    }

    private async Task<RpcInvocationResult<TResponse>> InvokeCoreAsync<TResponse>(
        RpcRequest request,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.PostAsJsonAsync("/api/rpc", request, JsonOptions, cancellationToken);
        var envelope = await response.Content.ReadFromJsonAsync<RpcEnvelope>(JsonOptions, cancellationToken);

        if (envelope is null)
        {
            return new RpcInvocationResult<TResponse>(
                false,
                Data: default,
                [new RpcErrorResponse("rpc_response_invalid", "RPC response payload was empty.")]);
        }

        if (!envelope.Succeeded)
        {
            return new RpcInvocationResult<TResponse>(false, default, envelope.Errors);
        }

        if (envelope.Data.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return new RpcInvocationResult<TResponse>(true, default, Array.Empty<RpcErrorResponse>());
        }

        var data = envelope.Data.Deserialize<TResponse>(JsonOptions);

        return new RpcInvocationResult<TResponse>(
            true,
            data,
            Array.Empty<RpcErrorResponse>());
    }

    private sealed record RpcEnvelope(bool Succeeded, JsonElement Data, IReadOnlyList<RpcErrorResponse> Errors);
}
