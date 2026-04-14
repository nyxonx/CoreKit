namespace CoreKit.AppHost.Contracts.Customers;

public sealed record UpdateCustomerRpcRequest(Guid Id, string Name, string? Email);
