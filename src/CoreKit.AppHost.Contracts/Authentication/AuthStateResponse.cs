namespace CoreKit.AppHost.Contracts.Authentication;

public sealed record AuthStateResponse(
    bool IsAuthenticated,
    string? UserName,
    IReadOnlyList<string> Roles,
    string? TenantIdentifier,
    string? TenantRole);
