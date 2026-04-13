namespace CoreKit.BuildingBlocks.Application;

public interface ICurrentTenantAuthorizationService
{
    Task<OperationError?> ValidateAccessAsync(CancellationToken cancellationToken = default);

    Task<OperationError?> ValidateAccessAsync(
        CurrentTenantAccessRequirement requirement,
        CancellationToken cancellationToken = default);
}
