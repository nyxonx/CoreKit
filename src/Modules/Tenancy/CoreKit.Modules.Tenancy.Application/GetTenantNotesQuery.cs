using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Tenancy.Application;

[RpcOperation("tenancy.notes.list")]
public sealed record GetTenantNotesQuery : IQuery<IReadOnlyList<TenantNoteDto>>;
