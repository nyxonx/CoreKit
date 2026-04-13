using CoreKit.BuildingBlocks.Presentation;

namespace CoreKit.AppHost.Server.Extensions;

public static class AppInitializationExtensions
{
    public static async Task<bool> InitializeCoreKitAppAsync(
        this WebApplication app,
        string[] args,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(args);

        await app.Services.InitializeRegisteredCoreKitModulesAsync(app.Configuration, cancellationToken);

        return !args.Contains("--provision-only", StringComparer.OrdinalIgnoreCase);
    }
}
