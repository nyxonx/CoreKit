namespace CoreKit.AppHost.Contracts.Customers;

public sealed record CreateCustomerRpcRequest(string Name, string? Email);
