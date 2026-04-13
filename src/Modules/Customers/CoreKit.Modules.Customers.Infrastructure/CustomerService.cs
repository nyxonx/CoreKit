using CoreKit.Modules.Customers.Application;
using CoreKit.Modules.Customers.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Customers.Infrastructure;

public sealed class CustomerService(CustomersDbContext dbContext) : ICustomerService
{
    public async Task<IReadOnlyList<CustomerDto>> GetCustomersAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.EnsureSchemaAsync(cancellationToken);

        return await dbContext.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.Name)
            .Select(customer => new CustomerDto(customer.Id, customer.Name, customer.Email))
            .ToListAsync(cancellationToken);
    }

    public async Task<CustomerDto?> GetCustomerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await dbContext.EnsureSchemaAsync(cancellationToken);

        return await dbContext.Customers
            .AsNoTracking()
            .Where(customer => customer.Id == id)
            .Select(customer => new CustomerDto(customer.Id, customer.Name, customer.Email))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<CustomerDto> CreateCustomerAsync(
        CreateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        await dbContext.EnsureSchemaAsync(cancellationToken);

        var customer = new Customer
        {
            Name = request.Name.Trim(),
            Email = string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim()
        };

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CustomerDto(customer.Id, customer.Name, customer.Email);
    }

    public async Task<CustomerDto?> UpdateCustomerAsync(
        UpdateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        await dbContext.EnsureSchemaAsync(cancellationToken);

        var customer = await dbContext.Customers.SingleOrDefaultAsync(
            entity => entity.Id == request.Id,
            cancellationToken);

        if (customer is null)
        {
            return null;
        }

        customer.Name = request.Name.Trim();
        customer.Email = string.IsNullOrWhiteSpace(request.Email)
            ? null
            : request.Email.Trim();

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CustomerDto(customer.Id, customer.Name, customer.Email);
    }
}
