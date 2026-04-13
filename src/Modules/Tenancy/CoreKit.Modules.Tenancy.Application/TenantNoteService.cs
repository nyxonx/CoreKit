using CoreKit.Modules.Tenancy.Domain;
using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Tenancy.Application;

public sealed class TenantNoteService(TenantAppDbContext tenantAppDbContext) : ITenantNoteService
{
    public async Task<IReadOnlyList<TenantNoteDto>> GetNotesAsync(CancellationToken cancellationToken = default)
    {
        await tenantAppDbContext.EnsureSchemaAsync(cancellationToken);

        return await tenantAppDbContext.Notes
            .AsNoTracking()
            .OrderBy(note => note.Value)
            .Select(note => new TenantNoteDto(note.Id, note.Value))
            .ToListAsync(cancellationToken);
    }

    public async Task<TenantNoteDto> AddNoteAsync(
        AddTenantNoteRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Value))
        {
            throw new InvalidOperationException("Tenant note value is required.");
        }

        await tenantAppDbContext.EnsureSchemaAsync(cancellationToken);

        var note = new TenantNote
        {
            Value = request.Value.Trim()
        };

        tenantAppDbContext.Notes.Add(note);
        await tenantAppDbContext.SaveChangesAsync(cancellationToken);

        return new TenantNoteDto(note.Id, note.Value);
    }
}
