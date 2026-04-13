using CoreKit.BuildingBlocks.Presentation;

namespace CoreKit.TestInfrastructure;

public sealed class NoOpAuditEventWriter : IAuditEventWriter
{
    public Task WriteAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
