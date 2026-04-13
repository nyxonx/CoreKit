using MediatR;
using Microsoft.Extensions.Logging;

namespace CoreKit.BuildingBlocks.Application;

public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling request {RequestType}.", typeof(TRequest).Name);

        var response = await next();

        if (response is IOperationResult operationResult && !operationResult.IsSuccess)
        {
            logger.LogWarning(
                "Request {RequestType} completed with {ErrorCount} error(s).",
                typeof(TRequest).Name,
                operationResult.Errors.Count);
        }
        else
        {
            logger.LogInformation("Request {RequestType} completed successfully.", typeof(TRequest).Name);
        }

        return response;
    }
}
