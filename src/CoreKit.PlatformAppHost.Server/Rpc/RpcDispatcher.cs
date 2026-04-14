using System.Text.Json;
using CoreKit.AppHost.Contracts.Rpc;
using CoreKit.BuildingBlocks.Application;
using CoreKit.BuildingBlocks.Presentation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoreKit.AppHost.Server.Rpc;

public sealed class RpcDispatcher(
    IMediator mediator,
    RpcOperationRegistry operationRegistry,
    ILogger<RpcDispatcher> logger,
    IAuditEventWriter auditEventWriter)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<RpcResponse> DispatchAsync(RpcRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var startedAt = DateTimeOffset.UtcNow;

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

        object? response;

        try
        {
            response = await mediator.Send(message, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception while dispatching RPC operation {Operation}.", request.Operation);
            await WriteAuditAsync(request.Operation, "error", startedAt, cancellationToken);
            CoreKitTelemetry.RpcRequests.Add(1, new KeyValuePair<string, object?>("operation", request.Operation), new KeyValuePair<string, object?>("outcome", "error"));
            CoreKitTelemetry.RpcDurationMs.Record(
                (DateTimeOffset.UtcNow - startedAt).TotalMilliseconds,
                new KeyValuePair<string, object?>("operation", request.Operation),
                new KeyValuePair<string, object?>("outcome", "error"));

            return CreateErrorResponse(
                "rpc_unhandled_error",
                "An unexpected error occurred while processing the RPC operation.");
        }

        if (response is not IOperationResult operationResult)
        {
            return CreateErrorResponse(
                "rpc_handler_result_invalid",
                $"RPC operation '{request.Operation}' returned an unsupported response type.");
        }

        var rpcResponse = operationResult.IsSuccess
            ? new RpcResponse(true, operationResult.Value, Array.Empty<RpcErrorResponse>())
            : new RpcResponse(
                false,
                Data: null,
                operationResult.Errors
                    .Select(error => new RpcErrorResponse(error.Code, error.Message))
                    .ToArray());

        var outcome = rpcResponse.Succeeded ? "success" : "failure";
        await WriteAuditAsync(request.Operation, outcome, startedAt, cancellationToken, rpcResponse.Errors);
        CoreKitTelemetry.RpcRequests.Add(1, new KeyValuePair<string, object?>("operation", request.Operation), new KeyValuePair<string, object?>("outcome", outcome));
        CoreKitTelemetry.RpcDurationMs.Record(
            (DateTimeOffset.UtcNow - startedAt).TotalMilliseconds,
            new KeyValuePair<string, object?>("operation", request.Operation),
            new KeyValuePair<string, object?>("outcome", outcome));

        return rpcResponse;
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

    private Task WriteAuditAsync(
        string operation,
        string outcome,
        DateTimeOffset startedAt,
        CancellationToken cancellationToken,
        IReadOnlyList<RpcErrorResponse>? errors = null)
    {
        return auditEventWriter.WriteAsync(
            new AuditEvent(
                "rpc",
                operation,
                outcome,
                Details: new Dictionary<string, object?>
                {
                    ["durationMs"] = (DateTimeOffset.UtcNow - startedAt).TotalMilliseconds,
                    ["errorCodes"] = errors?.Select(error => error.Code).ToArray()
                }),
            cancellationToken);
    }
}
