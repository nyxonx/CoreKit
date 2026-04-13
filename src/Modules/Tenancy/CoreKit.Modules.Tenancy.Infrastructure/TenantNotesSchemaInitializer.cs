using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public static class TenantNotesSchemaInitializer
{
    public static Task EnsureSchemaAsync(
        this TenantAppDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        return dbContext.Database.ExecuteSqlRawAsync(
            """
            CREATE TABLE IF NOT EXISTS "TenantNotes" (
                "Id" TEXT NOT NULL CONSTRAINT "PK_TenantNotes" PRIMARY KEY,
                "Value" TEXT NOT NULL
            );
            """,
            cancellationToken);
    }
}
