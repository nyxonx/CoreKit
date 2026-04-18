using CoreKit.AppHost.Contracts.Rpc;

namespace CoreKit.PlatformAppHost.Server.Rpc;

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
                    return RpcHttpResults.FromResponse(response);
                })
            .WithName("DispatchRpcOperation")
            .WithTags("RPC");

        return endpoints;
    }
}
