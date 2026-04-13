namespace CoreKit.BuildingBlocks.Application;

public interface ICurrentTenantAuthorizationService
{
    Task<OperationError?> ValidateAccessAsync(CancellationToken cancellationToken = default);
}
