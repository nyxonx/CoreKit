using CoreKit.AppHost.Contracts.Rpc;
using Microsoft.AspNetCore.Http;

namespace CoreKit.AppHost.Server.Rpc;

internal static class RpcHttpResults
{
    public static IResult FromResponse(RpcResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        if (response.Succeeded)
        {
            return Results.Ok(response);
        }

        var statusCode = GetStatusCode(response.Errors);
        return Results.Json(response, statusCode: statusCode);
    }

    private static int GetStatusCode(IReadOnlyList<RpcErrorResponse> errors)
    {
        var codes = errors.Select(error => error.Code).ToArray();

        if (codes.Any(code => code.EndsWith("_not_found", StringComparison.Ordinal)))
        {
            return StatusCodes.Status404NotFound;
        }

        if (codes.Any(code => code.EndsWith("_conflict", StringComparison.Ordinal)))
        {
            return StatusCodes.Status409Conflict;
        }

        if (codes.Any(code => code == "authentication_required"))
        {
            return StatusCodes.Status401Unauthorized;
        }

        if (codes.Any(code => code == "tenant_membership_required"))
        {
            return StatusCodes.Status403Forbidden;
        }

        if (codes.Any(code => code == "tenant_role_required"))
        {
            return StatusCodes.Status403Forbidden;
        }

        if (codes.Any(
                code => code is "validation_error"
                    or "rpc_operation_required"
                    or "rpc_operation_not_found"
                    or "rpc_payload_invalid"
                    or "tenant_context_required"))
        {
            return StatusCodes.Status400BadRequest;
        }

        if (codes.Any(code => code is "rpc_handler_result_invalid" or "rpc_unhandled_error"))
        {
            return StatusCodes.Status500InternalServerError;
        }

        return StatusCodes.Status400BadRequest;
    }
}
