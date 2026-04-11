namespace CoreKit.Modules.Tenancy.Domain;

public sealed class TenantNote
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Value { get; set; } = string.Empty;
}
