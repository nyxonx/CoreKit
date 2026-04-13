namespace CoreKit.Modules.Identity.Domain;

public sealed class AppUserTenantMembership
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserId { get; set; } = string.Empty;

    public string TenantIdentifier { get; set; } = string.Empty;

    public string Role { get; set; } = "Member";

    public bool IsActive { get; set; } = true;

    public AppUser? User { get; set; }
}
