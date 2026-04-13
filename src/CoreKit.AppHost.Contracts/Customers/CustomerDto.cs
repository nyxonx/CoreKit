namespace CoreKit.AppHost.Contracts.Customers;

public sealed record CustomerDto(Guid Id, string Name, string? Email);
