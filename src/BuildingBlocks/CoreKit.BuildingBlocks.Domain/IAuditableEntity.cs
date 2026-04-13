namespace CoreKit.BuildingBlocks.Domain;

public interface IAuditableEntity
{
    string? TenantIdentifier { get; set; }

    string? CreatedByUserId { get; set; }

    string? ModifiedByUserId { get; set; }

    DateTimeOffset? CreatedUtc { get; set; }

    DateTimeOffset? ModifiedUtc { get; set; }
}
