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
    }
}
