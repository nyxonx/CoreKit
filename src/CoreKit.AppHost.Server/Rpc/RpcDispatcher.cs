using System.Text.Json;
using CoreKit.AppHost.Contracts.Rpc;
using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.AppHost.Server.Rpc;

public sealed class RpcDispatcher(IMediator mediator, RpcOperationRegistry operationRegistry)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<RpcResponse> DispatchAsync(RpcRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Operation))
        {
            return CreateErrorResponse("rpc_operation_required", "The RPC operation name is required.");
        }

        if (!operationRegistry.TryGet(request.Operation, out var operation))
        {
            return CreateErrorResponse(
                "rpc_operation_not_found",
                $"RPC operation '{request.Operation}' is not registered.");
        }

        object? message;

        try
        {
            message = DeserializePayload(request.Payload, operation.RequestType);
        }
        catch (JsonException)
        {
            return CreateErrorResponse(
                "rpc_payload_invalid",
                $"RPC payload for operation '{request.Operation}' is invalid.");
        }

        if (message is null)
        {
            return CreateErrorResponse(
                "rpc_payload_invalid",
                $"RPC payload for operation '{request.Operation}' could not be deserialized.");
        }

        var response = await mediator.Send(message, cancellationToken);

        if (response is not IOperationResult operationResult)
        {
            return CreateErrorResponse(
                "rpc_handler_result_invalid",
                $"RPC operation '{request.Operation}' returned an unsupported response type.");
        }

        return operationResult.IsSuccess
            ? new RpcResponse(true, operationResult.Value, Array.Empty<RpcErrorResponse>())
            : new RpcResponse(
                false,
                Data: null,
                operationResult.Errors
                    .Select(error => new RpcErrorResponse(error.Code, error.Message))
                    .ToArray());
    }

    private static object? DeserializePayload(JsonElement payload, Type requestType)
    {
        if (payload.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return Activator.CreateInstance(requestType);
        }

        return JsonSerializer.Deserialize(payload.GetRawText(), requestType, JsonOptions);
    }

    private static RpcResponse CreateErrorResponse(string code, string message) =>
        new(false, Data: null, [new RpcErrorResponse(code, message)]);
}
