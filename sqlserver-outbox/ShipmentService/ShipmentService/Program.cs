using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ShipmentService;
using ShipmentService.DependencyInjection;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .ConfigureResource(r => r.AddService("ShipmentService"))
    .AddSource(ShipmentServiceInstrumentation.ActivitySourceName)
    .SetSampler<AlwaysOnSampler>()
    .AddSqlClientInstrumentation(options =>
    {
        options.SetDbStatementForText = true;
        options.RecordException = true;
    })
    .AddConsoleExporter()
    .AddOtlpExporter()
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services
            .AddSingleton<ShipmentServiceInstrumentation>()
            .AddShipmentServices(context.Configuration)
            .AddHostedService<Worker>();
    })
    .Build();

host.Run();
