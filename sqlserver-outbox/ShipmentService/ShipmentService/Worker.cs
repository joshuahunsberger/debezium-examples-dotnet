using System.Diagnostics;
using System.Text;
using Confluent.Kafka;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using ShipmentService.Configuration;
using ShipmentService.Events;
using ShipmentService.Services;

namespace ShipmentService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly KafkaConfiguration _kafkaConfiguration;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ActivitySource _activitySource;

    public Worker(ILogger<Worker> logger, KafkaConfiguration kafkaConfiguration, IServiceScopeFactory serviceScopeFactory, ShipmentServiceInstrumentation shipmentServiceInstrumentation)
    {
        _logger = logger;
        _kafkaConfiguration = kafkaConfiguration;
        _serviceScopeFactory = serviceScopeFactory;
        _activitySource = shipmentServiceInstrumentation.ShipmentServiceActivitySource;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            GroupId = "shipment-service",
            BootstrapServers = _kafkaConfiguration.Broker,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
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
                var headers = consumeResult.Message.Headers;
                using var activity = CreateOpenTelemetrySpan(headers);

                using var scope = _serviceScopeFactory.CreateScope();
                var messageLogService = scope.ServiceProvider.GetRequiredService<IMessageLogService>();
                await messageLogService.LogProcessedMessage(key);
                var orderReceivedService = scope.ServiceProvider.GetRequiredService<IOrderReceivedService>();
                await orderReceivedService.HandleReceivedOrder(orderEvent);
                consumer.Commit(consumeResult);
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }

        Activity? CreateOpenTelemetrySpan(Headers headers)
        {
            var propagator = Propagators.DefaultTextMapPropagator;
            var parentContext = propagator.Extract(default, headers, ExtractTraceInfoFromHeaders);
            Baggage.Current = parentContext.Baggage;
            var activity = _activitySource.StartActivity("Process order message", ActivityKind.Server, parentContext.ActivityContext);
            return activity;
        }

        static IEnumerable<string> ExtractTraceInfoFromHeaders(Headers headers, string key)
        {
            if (headers.Any(h => h.Key == key))
            {
                var matchingHeader = headers.First(h => h.Key == key);
                var bytes = matchingHeader.GetValueBytes();
                return new[] { Encoding.UTF8.GetString(bytes) };
            }

            return Enumerable.Empty<string>();
        }
    }
}
