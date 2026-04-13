namespace CoreKit.AppHost.Contracts.Identity;

public static class IdentityRpcOperations
{
    public const string ListMemberships = "identity.memberships.list";

    public const string UpsertMembership = "identity.memberships.upsert";

    public const string ListMembershipsForTenant = "identity.platform-memberships.list";

    public const string UpsertMembershipForTenant = "identity.platform-memberships.upsert";

    public const string SetMembershipActivationForTenant = "identity.platform-memberships.activation";
}
