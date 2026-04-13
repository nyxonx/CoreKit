namespace CoreKit.BuildingBlocks.Presentation;

public sealed record AuditEvent(
    string Category,
    string Action,
    string Outcome,
    string? Subject = null,
    IReadOnlyDictionary<string, object?>? Details = null);
