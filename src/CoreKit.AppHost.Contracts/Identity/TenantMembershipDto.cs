namespace CoreKit.AppHost.Contracts.Identity;

public sealed record TenantMembershipDto(
    string UserName,
    string Role,
    bool IsActive);
