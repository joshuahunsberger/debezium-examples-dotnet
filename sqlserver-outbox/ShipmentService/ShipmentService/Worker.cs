using Confluent.Kafka;
using ShipmentService.Configuration;
using ShipmentService.Events;
using ShipmentService.Services;

namespace ShipmentService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly KafkaConfiguration _kafkaConfiguration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Worker(ILogger<Worker> logger, KafkaConfiguration kafkaConfiguration, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _kafkaConfiguration = kafkaConfiguration;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            GroupId = "shipment-service",
            BootstrapServers = _kafkaConfiguration.Broker,
            EnableAutoCommit = false
        };
        using var consumer = new ConsumerBuilder<Guid, OrderCreatedEvent>(consumerConfig)
            .SetKeyDeserializer(new JsonDeserializer<Guid>())
            .SetValueDeserializer(new JsonDeserializer<OrderCreatedEvent>())
            .Build();

        consumer.Subscribe("Order.events");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);

                var key = consumeResult.Message.Key;
                var orderEvent = consumeResult.Message.Value;
                using var scope = _serviceScopeFactory.CreateScope();
                var messageLogService = scope.ServiceProvider.GetRequiredService<IMessageLogService>();
                await messageLogService.LogProcessedMessage(key);
                consumer.Commit(consumeResult);
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }

    }
}
