using CoreKit.AppHost.Contracts.Tenancy;

namespace CoreKit.AppHost.Client.Services;

public interface ITenancyModuleClient
{
    Task<RpcInvocationResult<IReadOnlyList<TenantNoteDto>>> GetNotesAsync(
        CancellationToken cancellationToken = default);

    Task<RpcInvocationResult<TenantNoteDto>> AddNoteAsync(
        AddTenantNoteRpcRequest request,
        CancellationToken cancellationToken = default);
}
