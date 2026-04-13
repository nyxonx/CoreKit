using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Customers.Infrastructure;

internal static class CustomersSchemaInitializer
{
    public static Task EnsureSchemaAsync(
        this CustomersDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        return dbContext.Database.ExecuteSqlRawAsync(
            """
            CREATE TABLE IF NOT EXISTS "Customers" (
                "Id" TEXT NOT NULL CONSTRAINT "PK_Customers" PRIMARY KEY,
                "Name" TEXT NOT NULL,
                "Email" TEXT NULL
            );
            """,
            cancellationToken);
    }
}
