using CoreKit.Modules.Tenancy.Application;
using CoreKit.Modules.Tenancy.Domain;
using CoreKit.Modules.Tenancy.Infrastructure;
using CoreKit.Modules.Customers.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreKit.Modules.Tenancy.Tests;

public sealed class TenantDataAccessTests
{
    [Fact]
    public async Task TenantDbContextFactory_IsolatesDataAcrossTenantDatabases()
    {
        var tempRoot = Path.Combine(Path.GetTempPath(), "corekit-tenant-data-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var firstTenantDatabase = Path.Combine(tempRoot, "tenant-a.db");
            var secondTenantDatabase = Path.Combine(tempRoot, "tenant-b.db");

            await AddNoteAsync("tenant-a", $"Data Source={firstTenantDatabase}", "note-from-a");
            await AddNoteAsync("tenant-b", $"Data Source={secondTenantDatabase}", "note-from-b");

            var notesForTenantA = await ReadNotesAsync("tenant-a", $"Data Source={firstTenantDatabase}");
            var notesForTenantB = await ReadNotesAsync("tenant-b", $"Data Source={secondTenantDatabase}");

            Assert.Single(notesForTenantA);
            Assert.Single(notesForTenantB);
            Assert.Equal("note-from-a", notesForTenantA[0]);
            Assert.Equal("note-from-b", notesForTenantB[0]);
        }
        finally
        {
            if (Directory.Exists(tempRoot))
            {
                TryDeleteDirectory(tempRoot);
            }
        }
    }

    [Fact]
    public void TenantConnectionStringProvider_Throws_WhenTenantContextIsMissing()
    {
        var accessor = new TenantContextAccessor();
        var provider = new TenantConnectionStringProvider(accessor);

        var exception = Assert.Throws<InvalidOperationException>(() => provider.GetRequiredConnectionString());

        Assert.Contains("Tenant context is not available", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task TenantNoteService_AddsAndReadsNotes_FromTenantDatabase()
    {
        var tempRoot = Path.Combine(Path.GetTempPath(), "corekit-tenant-note-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var databasePath = Path.Combine(tempRoot, "tenant-notes.db");
            var service = CreateNoteService("tenant-notes", $"Data Source={databasePath}");

            var createdNote = await service.AddNoteAsync(new AddTenantNoteRequest("hello tenant"));
            var notes = await service.GetNotesAsync();

            Assert.Equal("hello tenant", createdNote.Value);
            Assert.Single(notes);
            Assert.Equal("hello tenant", notes[0].Value);
        }
        finally
        {
            if (Directory.Exists(tempRoot))
            {
                TryDeleteDirectory(tempRoot);
            }
        }
    }

    [Fact]
    public async Task TenantProvisioningService_CreatesModuleSchemas_AndSeedsTenantMetadata()
    {
        var tempRoot = Path.Combine(Path.GetTempPath(), "corekit-tenant-provisioning-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var databasePath = Path.Combine(tempRoot, "tenant-provisioning.db");
            await using var provider = CreateProvisioningServices();
            await using var scope = provider.CreateAsyncScope();

            var provisioningService = scope.ServiceProvider.GetRequiredService<TenantProvisioningService>();

            await provisioningService.ProvisionTenantAsync(
                new TenantCatalogEntry
                {
                    Identifier = "provisioned-tenant",
                    Name = "Provisioned Tenant",
                    Host = "provisioned.local",
                    IsActive = true,
                    ConnectionString = $"Data Source={databasePath}"
                });

            await using var connection = new SqliteConnection($"Data Source={databasePath}");
            await connection.OpenAsync();

            Assert.True(await TableExistsAsync(connection, "Customers"));
            Assert.True(await TableExistsAsync(connection, "TenantNotes"));
            Assert.True(await TableExistsAsync(connection, "TenantMetadata"));

            await using var command = connection.CreateCommand();
            command.CommandText = """SELECT "TenantIdentifier" FROM "TenantMetadata" WHERE "Id" = 1;""";

            var identifier = (string?)await command.ExecuteScalarAsync();

            Assert.Equal("provisioned-tenant", identifier);
        }
        finally
        {
            if (Directory.Exists(tempRoot))
            {
                TryDeleteDirectory(tempRoot);
            }
        }
    }

    private static async Task AddNoteAsync(string tenantIdentifier, string connectionString, string value)
    {
        var accessor = new TenantContextAccessor
        {
            TenantContext = new TenantContext
            {
                Identifier = tenantIdentifier,
                Name = tenantIdentifier,
                Host = $"{tenantIdentifier}.local",
                ConnectionString = connectionString
            }
        };

        var connectionStringProvider = new TenantConnectionStringProvider(accessor);
        var dbContextFactory = new TenantDbContextFactory(connectionStringProvider);
        var bootstrapper = new TenantDatabaseBootstrapper(accessor, CreateTenantProvisioningService());

        await bootstrapper.EnsureCreatedAsync();

        await using var dbContext = dbContextFactory.CreateDbContext();
        dbContext.Notes.Add(new TenantNote { Value = value });
        await dbContext.SaveChangesAsync();
    }

    private static async Task<List<string>> ReadNotesAsync(string tenantIdentifier, string connectionString)
    {
        var accessor = new TenantContextAccessor
        {
            TenantContext = new TenantContext
            {
                Identifier = tenantIdentifier,
                Name = tenantIdentifier,
                Host = $"{tenantIdentifier}.local",
                ConnectionString = connectionString
            }
        };

        var connectionStringProvider = new TenantConnectionStringProvider(accessor);
        var dbContextFactory = new TenantDbContextFactory(connectionStringProvider);

        await using var dbContext = dbContextFactory.CreateDbContext();

        return await dbContext.Notes
            .OrderBy(note => note.Value)
            .Select(note => note.Value)
            .ToListAsync();
    }

    private static ITenantNoteService CreateNoteService(string tenantIdentifier, string connectionString)
    {
        var accessor = new TenantContextAccessor
        {
            TenantContext = new TenantContext
            {
                Identifier = tenantIdentifier,
                Name = tenantIdentifier,
                Host = $"{tenantIdentifier}.local",
                ConnectionString = connectionString
            }
        };

        var connectionStringProvider = new TenantConnectionStringProvider(accessor);
        var dbContextFactory = new TenantDbContextFactory(connectionStringProvider);
        var dbContext = dbContextFactory.CreateDbContext();

        return new TenantNoteService(dbContext);
    }

    private static ServiceProvider CreateProvisioningServices()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddScoped<ITenantDatabaseMigration, TenantMetadataMigration>();
        services.AddScoped<ITenantDatabaseMigration, TenantNotesDatabaseMigration>();
        services.AddScoped<ITenantDatabaseMigration, CustomersDatabaseMigration>();
        services.AddScoped<ITenantSeedDataContributor, TenantMetadataSeedContributor>();
        services.AddScoped<TenantDatabaseMigrationRunner>();
        services.AddScoped<TenantSeedDataRunner>();
        services.AddDbContext<TenantCatalogDbContext>(
            options => options.UseSqlite($"Data Source={Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"))}.catalog.db"));
        services.AddScoped<TenantProvisioningService>();

        return services.BuildServiceProvider();
    }

    private static TenantProvisioningService CreateTenantProvisioningService()
    {
        var options = new DbContextOptionsBuilder<TenantCatalogDbContext>()
            .UseSqlite($"Data Source={Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"))}.catalog.db")
            .Options;

        var dbContext = new TenantCatalogDbContext(options);
        var migrationRunner = new TenantDatabaseMigrationRunner(
        [
            new TenantMetadataMigration(),
            new TenantNotesDatabaseMigration()
        ]);
        var seedRunner = new TenantSeedDataRunner(
        [
            new TenantMetadataSeedContributor()
        ]);

        return new TenantProvisioningService(dbContext, migrationRunner, seedRunner);
    }

    private static async Task<bool> TableExistsAsync(SqliteConnection connection, string tableName)
    {
        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT COUNT(*)
            FROM "sqlite_master"
            WHERE "type" = 'table' AND "name" = $tableName;
            """;
        command.Parameters.AddWithValue("$tableName", tableName);

        var result = (long)(await command.ExecuteScalarAsync() ?? 0L);
        return result > 0;
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            Directory.Delete(path, recursive: true);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
