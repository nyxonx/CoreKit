using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.Modules.Tenancy.Application;

public sealed class AddTenantNoteCommandHandler(ITenantNoteService tenantNoteService)
    : IRequestHandler<AddTenantNoteCommand, OperationResult<TenantNoteDto>>
{
    public async Task<OperationResult<TenantNoteDto>> Handle(
        AddTenantNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = await tenantNoteService.AddNoteAsync(
            new AddTenantNoteRequest(request.Value),
            cancellationToken);

        return OperationResult<TenantNoteDto>.Success(note);
    }
}
