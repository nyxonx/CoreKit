using System.Reflection;

namespace CoreKit.BuildingBlocks.Application;

public static class OperationResultFactory
{
    public static TResponse CreateInvalid<TResponse>(IReadOnlyList<OperationError> errors)
    {
        var invalidFactory = typeof(TResponse).GetMethod(
            nameof(OperationResult<object>.Invalid),
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            [typeof(IReadOnlyList<OperationError>)],
            modifiers: null);

        if (invalidFactory is null)
        {
            throw new InvalidOperationException(
                $"Response type '{typeof(TResponse).FullName}' does not expose a compatible Invalid factory.");
        }

        return (TResponse)(invalidFactory.Invoke(null, [errors])
            ?? throw new InvalidOperationException("Could not create an invalid operation result instance."));
    }
}
