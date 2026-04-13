using CoreKit.AppHost.Contracts.Rpc;

namespace CoreKit.AppHost.Server.Rpc;

public static class RpcEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapRpcEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapPost(
                "/api/rpc",
                async (RpcRequest request, RpcDispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    var response = await dispatcher.DispatchAsync(request, cancellationToken);
                    return response.Succeeded ? Results.Ok(response) : Results.BadRequest(response);
                })
            .WithName("DispatchRpcOperation")
            .WithTags("RPC");

        return endpoints;
    }
}
