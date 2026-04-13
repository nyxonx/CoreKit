using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Customers.Infrastructure;

internal static class CustomersSchemaInitializer
{
    public static Task EnsureSchemaAsync(
        this CustomersDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        return EnsureSchemaInternalAsync(dbContext, cancellationToken);
    }

    private static async Task EnsureSchemaInternalAsync(
        CustomersDbContext dbContext,
        CancellationToken cancellationToken)
    {
        await dbContext.Database.ExecuteSqlRawAsync(
            """
            CREATE TABLE IF NOT EXISTS "Customers" (
                "Id" TEXT NOT NULL CONSTRAINT "PK_Customers" PRIMARY KEY,
                "Name" TEXT NOT NULL,
                "Email" TEXT NULL,
                "TenantIdentifier" TEXT NULL,
                "CreatedByUserId" TEXT NULL,
                "ModifiedByUserId" TEXT NULL,
                "CreatedUtc" TEXT NULL,
                "ModifiedUtc" TEXT NULL
            );
            """,
            cancellationToken);

        await EnsureColumnAsync(dbContext, "TenantIdentifier", cancellationToken);
        await EnsureColumnAsync(dbContext, "CreatedByUserId", cancellationToken);
        await EnsureColumnAsync(dbContext, "ModifiedByUserId", cancellationToken);
        await EnsureColumnAsync(dbContext, "CreatedUtc", cancellationToken);
        await EnsureColumnAsync(dbContext, "ModifiedUtc", cancellationToken);
    }

    private static async Task EnsureColumnAsync(
        CustomersDbContext dbContext,
        string columnName,
        CancellationToken cancellationToken)
    {
        var connection = (SqliteConnection)dbContext.Database.GetDbConnection();
        var shouldClose = connection.State != System.Data.ConnectionState.Open;

        if (shouldClose)
        {
            await connection.OpenAsync(cancellationToken);
        }

        try
        {
            await using var existsCommand = connection.CreateCommand();
            existsCommand.CommandText =
                """
                SELECT COUNT(*)
                FROM pragma_table_info('Customers')
                WHERE name = $name;
                """;
            existsCommand.Parameters.AddWithValue("$name", columnName);

            var exists = Convert.ToInt32(await existsCommand.ExecuteScalarAsync(cancellationToken)) > 0;

            if (exists)
            {
                return;
            }

            await using var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = $"""ALTER TABLE "Customers" ADD COLUMN "{columnName}" TEXT NULL;""";
            await alterCommand.ExecuteNonQueryAsync(cancellationToken);
        }
        finally
        {
            if (shouldClose)
            {
                await connection.CloseAsync();
            }
        }
    }
}
