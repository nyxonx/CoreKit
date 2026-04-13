namespace CoreKit.BuildingBlocks.Application;

public interface IOperationResult
{
    bool IsSuccess { get; }

    object? Value { get; }

    IReadOnlyList<OperationError> Errors { get; }
}

public sealed class OperationResult<T> : IOperationResult
{
    private OperationResult(T? value, IReadOnlyList<OperationError> errors)
    {
        Value = value;
        Errors = errors;
    }

    public bool IsSuccess => Errors.Count == 0;

    public T? Value { get; }

    object? IOperationResult.Value => Value;

    public IReadOnlyList<OperationError> Errors { get; }

    public static OperationResult<T> Success(T value) => new(value, Array.Empty<OperationError>());

    public static OperationResult<T> Invalid(IReadOnlyList<OperationError> errors) => new(default, errors);

    public static OperationResult<T> Failure(string code, string message) =>
        Invalid([new OperationError(code, message)]);
}
