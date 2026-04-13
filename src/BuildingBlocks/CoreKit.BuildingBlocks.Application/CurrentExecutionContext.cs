namespace CoreKit.BuildingBlocks.Application;

public sealed record CurrentExecutionContext(
    string? UserId,
    string? UserName,
    bool IsAuthenticated,
    string? TenantIdentifier);
