using CoreKit.Modules.Tenancy.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantAppDbContext(DbContextOptions<TenantAppDbContext> options)
    : DbContext(options)
{
    public DbSet<TenantNote> Notes => Set<TenantNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        var note = modelBuilder.Entity<TenantNote>();

        note.ToTable("TenantNotes");
        note.HasKey(entity => entity.Id);
        note.Property(entity => entity.Value).HasMaxLength(256).IsRequired();
    }
}
