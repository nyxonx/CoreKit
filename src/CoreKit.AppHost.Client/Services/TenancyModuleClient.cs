using CoreKit.AppHost.Contracts.Tenancy;

namespace CoreKit.AppHost.Client.Services;

public sealed class TenancyModuleClient(RpcClient rpcClient)
    : RpcModuleClientBase(rpcClient), ITenancyModuleClient
{
    public Task<RpcInvocationResult<IReadOnlyList<TenantNoteDto>>> GetNotesAsync(
        CancellationToken cancellationToken = default) =>
        InvokeAsync<IReadOnlyList<TenantNoteDto>>(
            TenancyRpcOperations.GetNotes,
            cancellationToken: cancellationToken);

    public Task<RpcInvocationResult<TenantNoteDto>> AddNoteAsync(
        AddTenantNoteRpcRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return InvokeAsync<TenantNoteDto>(
            TenancyRpcOperations.AddNote,
            request,
            cancellationToken);
    }
}
