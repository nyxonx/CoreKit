using Microsoft.Data.Sqlite;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantMetadataMigration : ITenantDatabaseMigration
{
    public string Name => "2026-04-13-tenancy-metadata";

    public async Task ApplyAsync(
        TenantProvisioningContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        await using var connection = new SqliteConnection(context.Tenant.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS "TenantMetadata" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_TenantMetadata" PRIMARY KEY,
                "TenantIdentifier" TEXT NOT NULL,
                "TenantName" TEXT NOT NULL,
                "Host" TEXT NOT NULL,
                "ProvisionedUtc" TEXT NOT NULL
            );
            """;

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
