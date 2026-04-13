using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreKit.BuildingBlocks.Presentation;

public sealed class CoreKitModuleInitializationPipeline(
    IReadOnlyList<ICoreKitModule> modules,
    ILogger<CoreKitModuleInitializationPipeline> logger)
{
    public async Task InitializeAsync(
        IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        foreach (var module in modules)
        {
            logger.LogInformation("Initializing CoreKit module {ModuleName}.", module.Name);
            await module.InitializeAsync(services, configuration, cancellationToken);
        }
    }
}
