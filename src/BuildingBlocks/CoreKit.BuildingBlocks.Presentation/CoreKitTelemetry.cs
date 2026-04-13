using System.Diagnostics.Metrics;

namespace CoreKit.BuildingBlocks.Presentation;

public static class CoreKitTelemetry
{
    public const string MeterName = "CoreKit.AppHost";

    private static readonly Meter Meter = new(MeterName);

    public static Counter<long> RpcRequests { get; } =
        Meter.CreateCounter<long>("corekit.rpc.requests");

    public static Histogram<double> RpcDurationMs { get; } =
        Meter.CreateHistogram<double>("corekit.rpc.duration.ms");

    public static Counter<long> AuthEvents { get; } =
        Meter.CreateCounter<long>("corekit.auth.events");
}
