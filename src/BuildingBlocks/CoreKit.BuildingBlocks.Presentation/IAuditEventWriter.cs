namespace CoreKit.BuildingBlocks.Presentation;

public interface IAuditEventWriter
{
    Task WriteAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default);
}
