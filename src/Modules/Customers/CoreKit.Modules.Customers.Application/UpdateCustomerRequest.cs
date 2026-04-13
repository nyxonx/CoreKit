namespace CoreKit.Modules.Customers.Application;

public sealed record UpdateCustomerRequest(Guid Id, string Name, string? Email);
