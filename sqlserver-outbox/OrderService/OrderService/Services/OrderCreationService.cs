using System.Diagnostics;
using System.Text;
using System.Text.Json;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OrderService.Data;
using OrderService.Events;
using OrderService.Models;

namespace OrderService.Services
{
    public interface IOrderCreationService
    {
        Task CreateOrder(CreateOrderRequest request);
    }

    public class OrderCreationService : IOrderCreationService
    {
        private readonly OrdersContext _context;

        public OrderCreationService(OrdersContext context)
        {
            _context = context;
        }

        public async Task CreateOrder(CreateOrderRequest request)
        {
            var orderEntity = new PurchaseOrder
            {
                OrderDate = request.OrderDate,
                CustomerId = request.CustomerId,
                PurchaseOrderKey = Guid.NewGuid(),
                OrderLineItems = request.LineItems
                    .Select(li => new Data.OrderLineItem
                    {
                        Item = li.Item,
                        Quantity = li.Quantity,
                        Status = "NEW",
                        TotalPrice = li.TotalPrice,
                        OrderLineItemKey = Guid.NewGuid()
                    })
                    .ToList()
            };

            _context.Add(orderEntity);

            var orderCreatedEvent = new OrderCreatedEvent(orderEntity);

            var propagator = Propagators.DefaultTextMapPropagator;
            var activity = Activity.Current;
            var traceParentBuilder = new StringBuilder();
            if (activity is not null)
            {

                propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), traceParentBuilder, (sb, key, value) =>
                {
                    if (key.Equals("traceparent"))
                    {
                        sb.Append(value);
                    }
                });
            }

            var traceParent = traceParentBuilder.ToString();
            var outboxMessage = new OutboxEvent
            {
                AggregateType = "Order",
                Type = "OrderCreated",
                AggregateId = orderCreatedEvent.OrderKey.ToString(),
                Payload = JsonSerializer.Serialize(orderCreatedEvent),
                TraceParent = traceParent
            };
            _context.Add(outboxMessage);

            var invoiceCreatedEvent = new InvoiceCreatedEvent(orderEntity);

            var invoiceOutboxMessage = new OutboxEvent
            {
                AggregateType = "Customer",
                Type = "InvoiceCreated",
                AggregateId = invoiceCreatedEvent.CustomerId.ToString(),
                Payload = JsonSerializer.Serialize(invoiceCreatedEvent),
                TraceParent = traceParent
            };

            _context.Add(invoiceOutboxMessage);

            await _context.SaveChangesAsync();
        }
    }
}

