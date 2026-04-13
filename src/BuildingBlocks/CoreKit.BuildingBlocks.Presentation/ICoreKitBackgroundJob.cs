namespace CoreKit.BuildingBlocks.Presentation;

public interface ICoreKitBackgroundJob
{
    string Name { get; }

    TimeSpan Interval { get; }

    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
