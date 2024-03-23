using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace Movies.Api.Observability;

public sealed class ObservabilityService : IDisposable
{
    public const string ServiceName = "MoviesApi";

    public ActivitySource Tracer { get; }
    public Meter Recorder { get; }
    public Counter<long> IncomingRequestCounter { get; }

    public ObservabilityService()
    {
        var version = typeof(ObservabilityService).Assembly.GetName().Version?.ToString();
        Tracer = new ActivitySource(ServiceName, version);
        Recorder = new Meter(ServiceName, version);
        IncomingRequestCounter = Recorder.CreateCounter<long>("api.requests",
            description: "Requests receiveds");
    }

    public void Dispose()
    {
        Tracer.Dispose();
        Recorder.Dispose();
    }
}
