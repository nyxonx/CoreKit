namespace CoreKit.Modules.Identity.Application;

public sealed record TenantMembershipDto(
    string UserName,
    string Role,
    bool IsActive);
