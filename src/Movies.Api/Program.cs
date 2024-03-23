
using Movies.Api.Observability;
using Movies.Api.Services;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.OpenTelemetry;



var builder = WebApplication.CreateBuilder(args);

const string outputTemplate =
    "[{Level:w}]: {Timestamp:dd-MM-yyyy:HH:mm:ss} {MachineName} {EnvironmentName} {SourceContext} {Message}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .WriteTo.Console(outputTemplate: outputTemplate)
    .WriteTo.OpenTelemetry(opts =>
    {
        opts.IncludedData = IncludedData.SpecRequiredResourceAttributes;
        opts.ResourceAttributes = new Dictionary<string, object>
        {
            ["app"] = "webapi",
            ["runtime"] = "dotnet",
            ["service.name"] = "api"
        };
    })
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddSingleton<ObservabilityService>();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(ObservabilityService.ServiceName))
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource(ObservabilityService.ServiceName)
            .AddAspNetCoreInstrumentation(opts =>
            {
                opts.Filter = context =>
                {
                    var ignore = new[] { "/swagger" };
                    return !ignore.Any(s => context.Request.Path.ToString().Contains(s));
                };
            })
            .AddHttpClientInstrumentation(opts =>
            {
                opts.FilterHttpRequestMessage = req =>
                {
                    var ignore = new[] { "/loki/api" };
                    return !ignore.Any(s => req.RequestUri!.ToString().Contains(s));
                };
            })
            .AddOtlpExporter())
    .WithMetrics(metricsProviderBuilder =>
        metricsProviderBuilder
            .AddMeter(ObservabilityService.ServiceName)
            .AddRuntimeInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation().AddOtlpExporter());



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMovieService, MovieService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();
