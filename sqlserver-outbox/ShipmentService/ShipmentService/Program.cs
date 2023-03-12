using ShipmentService;
using ShipmentService.DependencyInjection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services
            .AddShipmentServices(context.Configuration)
            .AddHostedService<Worker>();
    })
    .Build();

host.Run();

