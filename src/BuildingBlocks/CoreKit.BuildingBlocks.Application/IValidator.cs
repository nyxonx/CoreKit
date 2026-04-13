namespace CoreKit.BuildingBlocks.Application;

public interface IValidator<in TRequest>
{
    Task<IReadOnlyList<OperationError>> ValidateAsync(
        TRequest request,
        CancellationToken cancellationToken = default);
}
