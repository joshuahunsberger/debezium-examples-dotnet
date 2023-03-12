using Microsoft.EntityFrameworkCore;
using ShipmentService.Configuration;
using ShipmentService.Data;
using ShipmentService.Services;

namespace ShipmentService.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShipmentServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Shipments");
        var kafkaConfiguration = new KafkaConfiguration();

        configuration.GetSection("Kafka").Bind(kafkaConfiguration);
        return services
            .AddDbContext<ShipmentsContext>(builder => builder.UseSqlServer(connectionString))
            .AddScoped<IMessageLogService, MessageLogService>()
            .AddSingleton(kafkaConfiguration);
    }
}

