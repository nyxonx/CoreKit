using CoreKit.BuildingBlocks.Domain;

namespace CoreKit.Modules.Customers.Domain;

public sealed class Customer : IAuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? TenantIdentifier { get; set; }

    public string? CreatedByUserId { get; set; }

    public string? ModifiedByUserId { get; set; }

    public DateTimeOffset? CreatedUtc { get; set; }

    public DateTimeOffset? ModifiedUtc { get; set; }
}
