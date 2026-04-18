using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CoreKit.Modules.Tenancy.Infrastructure;

public static class LocalSqliteConnectionStringResolver
{
    private const string DatabaseRootKey = "Tenancy:DatabaseRoot";

    public static string Resolve(
        IConfiguration configuration,
        string configKey,
        string fallbackFileName)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(configKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(fallbackFileName);

        var configuredValue = configuration.GetConnectionString(configKey);
        return ResolveValue(configuration, configuredValue, fallbackFileName);
    }

    public static string ResolveFileName(
        IConfiguration configuration,
        string fileName)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var databaseRoot = configuration[DatabaseRootKey];
        var dataSource = string.IsNullOrWhiteSpace(databaseRoot)
            ? fileName
            : Path.Combine(databaseRoot, fileName);

        var builder = new SqliteConnectionStringBuilder
        {
            DataSource = dataSource
        };

        return builder.ToString();
    }

    private static string ResolveValue(
        IConfiguration configuration,
        string? configuredValue,
        string fallbackFileName)
    {
        if (string.IsNullOrWhiteSpace(configuredValue))
        {
            return ResolveFileName(configuration, fallbackFileName);
        }

        if (!configuredValue.Contains('='))
        {
            return ResolveFileName(configuration, configuredValue);
        }

        try
        {
            var builder = new SqliteConnectionStringBuilder(configuredValue);

            if (string.IsNullOrWhiteSpace(builder.DataSource))
            {
                return ResolveFileName(configuration, fallbackFileName);
            }

            var directory = Path.GetDirectoryName(builder.DataSource);
            if (string.IsNullOrWhiteSpace(directory))
            {
                return ResolveFileName(configuration, builder.DataSource);
            }

            return builder.ToString();
        }
        catch (ArgumentException)
        {
            return ResolveFileName(configuration, fallbackFileName);
        }
    }
}
