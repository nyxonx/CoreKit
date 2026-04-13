using MediatR;

namespace CoreKit.BuildingBlocks.Application;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validatorList = validators.ToArray();

        if (validatorList.Length == 0)
        {
            return await next();
        }

        var validationErrors = new List<OperationError>();

        foreach (var validator in validatorList)
        {
            var errors = await validator.ValidateAsync(request, cancellationToken);
            validationErrors.AddRange(errors);
        }

        if (validationErrors.Count == 0)
        {
            return await next();
        }

        return OperationResultFactory.CreateInvalid<TResponse>(validationErrors);
    }
}
