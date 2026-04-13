using Microsoft.Data.Sqlite;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public sealed class TenantDatabaseMigrationRunner(IEnumerable<ITenantDatabaseMigration> migrations)
{
    private const string HistoryTableName = "__CoreKitTenantMigrations";

    public async Task MigrateAsync(
        TenantProvisioningContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        await using var connection = new SqliteConnection(context.Tenant.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await EnsureHistoryTableAsync(connection, HistoryTableName, cancellationToken);
        var appliedMigrations = await GetAppliedEntriesAsync(connection, HistoryTableName, cancellationToken);

        foreach (var migration in migrations.OrderBy(item => item.Name, StringComparer.Ordinal))
        {
            if (appliedMigrations.Contains(migration.Name))
            {
                continue;
            }

            await migration.ApplyAsync(context, cancellationToken);
            await MarkEntryAppliedAsync(connection, HistoryTableName, migration.Name, cancellationToken);
        }
    }

    private static async Task EnsureHistoryTableAsync(
        SqliteConnection connection,
        string tableName,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText =
            $"""
            CREATE TABLE IF NOT EXISTS "{tableName}" (
                "Name" TEXT NOT NULL CONSTRAINT "PK_{tableName}" PRIMARY KEY,
                "AppliedUtc" TEXT NOT NULL
            );
            """;

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task<HashSet<string>> GetAppliedEntriesAsync(
        SqliteConnection connection,
        string tableName,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = $"""SELECT "Name" FROM "{tableName}";""";

        var appliedEntries = new HashSet<string>(StringComparer.Ordinal);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            appliedEntries.Add(reader.GetString(0));
        }

        return appliedEntries;
    }

    private static async Task MarkEntryAppliedAsync(
        SqliteConnection connection,
        string tableName,
        string name,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText =
            $"""
            INSERT INTO "{tableName}" ("Name", "AppliedUtc")
            VALUES ($name, $appliedUtc);
            """;
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$appliedUtc", DateTimeOffset.UtcNow.ToString("O"));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
