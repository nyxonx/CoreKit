using System.Text.Json;

namespace CoreKit.AppHost.Contracts.Rpc;

public sealed record RpcRequest(string Operation, JsonElement Payload);
