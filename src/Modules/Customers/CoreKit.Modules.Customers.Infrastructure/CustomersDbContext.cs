using CoreKit.Modules.Customers.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Customers.Infrastructure;

public sealed class CustomersDbContext(DbContextOptions<CustomersDbContext> options)
    : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        var customer = modelBuilder.Entity<Customer>();

        customer.ToTable("Customers");
        customer.HasKey(entity => entity.Id);
        customer.Property(entity => entity.Name).HasMaxLength(128).IsRequired();
        customer.Property(entity => entity.Email).HasMaxLength(256);
        customer.Property(entity => entity.TenantIdentifier).HasMaxLength(64);
        customer.Property(entity => entity.CreatedByUserId).HasMaxLength(128);
        customer.Property(entity => entity.ModifiedByUserId).HasMaxLength(128);
        customer.Property(entity => entity.CreatedUtc);
        customer.Property(entity => entity.ModifiedUtc);
    }
}
