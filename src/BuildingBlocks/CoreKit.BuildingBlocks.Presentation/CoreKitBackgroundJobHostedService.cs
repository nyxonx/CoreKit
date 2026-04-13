using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreKit.BuildingBlocks.Presentation;

public sealed class CoreKitBackgroundJobHostedService(
    CoreKitBackgroundJobRegistry registry,
    ILogger<CoreKitBackgroundJobHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (registry.Jobs.Count == 0)
        {
            logger.LogInformation("No CoreKit background jobs are registered.");
            return;
        }

        var jobTasks = registry.Jobs
            .Select(job => RunJobLoopAsync(job, stoppingToken))
            .ToArray();

        await Task.WhenAll(jobTasks);
    }

    private async Task RunJobLoopAsync(ICoreKitBackgroundJob job, CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "Starting background job {JobName} with interval {Interval}.",
            job.Name,
            job.Interval);

        await ExecuteJobSafelyAsync(job, stoppingToken);

        using var timer = new PeriodicTimer(job.Interval);

        while (!stoppingToken.IsCancellationRequested
               && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await ExecuteJobSafelyAsync(job, stoppingToken);
        }
    }

    private async Task ExecuteJobSafelyAsync(ICoreKitBackgroundJob job, CancellationToken stoppingToken)
    {
        var startedAt = DateTimeOffset.UtcNow;

        try
        {
            await job.ExecuteAsync(stoppingToken);

            logger.LogInformation(
                "Background job {JobName} completed in {DurationMs} ms.",
                job.Name,
                (DateTimeOffset.UtcNow - startedAt).TotalMilliseconds);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Background job {JobName} was canceled.", job.Name);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Background job {JobName} failed.", job.Name);
        }
    }
}
