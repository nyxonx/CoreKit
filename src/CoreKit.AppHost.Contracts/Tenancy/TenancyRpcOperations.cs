namespace CoreKit.AppHost.Contracts.Tenancy;

public static class TenancyRpcOperations
{
    public const string AddNote = "tenancy.notes.add";

    public const string CreateTenant = "tenancy.catalog.create";

    public const string GetNotes = "tenancy.notes.list";

    public const string GetTenants = "tenancy.catalog.list";
}
