namespace CoreKit.BuildingBlocks.Presentation;

public sealed class CoreKitBackgroundJobRegistry(IEnumerable<ICoreKitBackgroundJob> jobs)
{
    public IReadOnlyList<ICoreKitBackgroundJob> Jobs { get; } = jobs.ToArray();
}
