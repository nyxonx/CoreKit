using CoreKit.Modules.Identity.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Identity.Infrastructure;

public sealed class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public DbSet<AppUserTenantMembership> UserTenantMemberships => Set<AppUserTenantMembership>();

    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.OnModelCreating(builder);

        var membership = builder.Entity<AppUserTenantMembership>();

        membership.ToTable("UserTenantMemberships");
        membership.HasKey(entity => entity.Id);
        membership.Property(entity => entity.UserId).IsRequired();
        membership.Property(entity => entity.TenantIdentifier).HasMaxLength(64).IsRequired();
        membership.Property(entity => entity.Role).HasMaxLength(64).IsRequired();
        membership.HasIndex(entity => new { entity.UserId, entity.TenantIdentifier }).IsUnique();
        membership.HasOne(entity => entity.User)
            .WithMany()
            .HasForeignKey(entity => entity.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
