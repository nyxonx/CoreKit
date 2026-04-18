namespace CoreKit.AppHost.Contracts.Authentication;

public sealed record PlatformAuthStateResponse(
    bool IsAuthenticated,
    string? UserName,
    IReadOnlyList<string> Roles);
