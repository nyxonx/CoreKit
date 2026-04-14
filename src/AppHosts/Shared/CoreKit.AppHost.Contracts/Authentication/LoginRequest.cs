namespace CoreKit.AppHost.Contracts.Authentication;

public sealed record LoginRequest(
    string UserName,
    string Password,
    bool RememberMe);
