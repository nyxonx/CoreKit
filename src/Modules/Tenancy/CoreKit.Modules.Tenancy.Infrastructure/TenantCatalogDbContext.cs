using CoreKit.Modules.Tenancy.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantCatalogDbContext(DbContextOptions<TenantCatalogDbContext> options)
    : DbContext(options)
{
    public DbSet<TenantCatalogEntry> Tenants => Set<TenantCatalogEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        var tenant = modelBuilder.Entity<TenantCatalogEntry>();

        tenant.ToTable("Tenants");
        tenant.HasKey(entry => entry.Id);
        tenant.Property(entry => entry.Identifier).HasMaxLength(64).IsRequired();
        tenant.Property(entry => entry.Name).HasMaxLength(128).IsRequired();
        tenant.Property(entry => entry.Host).HasMaxLength(256).IsRequired();
        tenant.Property(entry => entry.ConnectionString).HasMaxLength(512).IsRequired();
        tenant.HasIndex(entry => entry.Identifier).IsUnique();
        tenant.HasIndex(entry => entry.Host).IsUnique();
    }
}
