using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Tenancy.Application;

public sealed class GetTenantNotesQueryHandler(ITenantNoteService tenantNoteService)
    : IRequestHandler<GetTenantNotesQuery, OperationResult<IReadOnlyList<TenantNoteDto>>>
{
    public async Task<OperationResult<IReadOnlyList<TenantNoteDto>>> Handle(
        GetTenantNotesQuery request,
        CancellationToken cancellationToken)
    {
        var notes = await tenantNoteService.GetNotesAsync(cancellationToken);
        return OperationResult<IReadOnlyList<TenantNoteDto>>.Success(notes);
    }
}
