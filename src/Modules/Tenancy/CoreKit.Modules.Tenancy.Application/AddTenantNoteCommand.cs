using CoreKit.BuildingBlocks.Application;

namespace CoreKit.Modules.Tenancy.Application;

[RpcOperation("tenancy.notes.add")]
public sealed record AddTenantNoteCommand(string Value) : ICommand<TenantNoteDto>;
