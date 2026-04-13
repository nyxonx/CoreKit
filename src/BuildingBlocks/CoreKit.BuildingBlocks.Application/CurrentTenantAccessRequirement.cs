namespace CoreKit.BuildingBlocks.Application;

public sealed record CurrentTenantAccessRequirement(params string[] AllowedRoles)
{
    public static readonly CurrentTenantAccessRequirement Membership = new();
}
