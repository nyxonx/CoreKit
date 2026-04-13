namespace CoreKit.Modules.Customers.Application;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerDto>> GetCustomersAsync(CancellationToken cancellationToken = default);

    Task<CustomerDto?> GetCustomerAsync(Guid id, CancellationToken cancellationToken = default);

    Task<CustomerDto> CreateCustomerAsync(
        CreateCustomerRequest request,
        CancellationToken cancellationToken = default);

    Task<CustomerDto?> UpdateCustomerAsync(
        UpdateCustomerRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default);
}
