namespace CoreKit.Modules.Tenancy.Application;

public interface ITenantNoteService
{
    Task<IReadOnlyList<TenantNoteDto>> GetNotesAsync(CancellationToken cancellationToken = default);

    Task<TenantNoteDto> AddNoteAsync(AddTenantNoteRequest request, CancellationToken cancellationToken = default);
}
