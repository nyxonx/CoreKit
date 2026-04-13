using Microsoft.Data.Sqlite;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantMetadataSeedContributor : ITenantSeedDataContributor
{
    public string Name => "2026-04-13-tenancy-metadata-seed";

    public async Task SeedAsync(
        TenantProvisioningContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        await using var connection = new SqliteConnection(context.Tenant.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO "TenantMetadata" ("Id", "TenantIdentifier", "TenantName", "Host", "ProvisionedUtc")
            VALUES (1, $identifier, $name, $host, $provisionedUtc)
            ON CONFLICT("Id") DO UPDATE SET
                "TenantIdentifier" = excluded."TenantIdentifier",
                "TenantName" = excluded."TenantName",
                "Host" = excluded."Host";
            """;
        command.Parameters.AddWithValue("$identifier", context.Tenant.Identifier);
        command.Parameters.AddWithValue("$name", context.Tenant.Name);
        command.Parameters.AddWithValue("$host", context.Tenant.Host);
        command.Parameters.AddWithValue("$provisionedUtc", DateTimeOffset.UtcNow.ToString("O"));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
