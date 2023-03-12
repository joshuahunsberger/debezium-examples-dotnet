using System;
using ShipmentService.Data;
using ShipmentService.Events;

namespace ShipmentService.Services;

public interface IOrderReceivedService
{
    Task HandleReceivedOrder(OrderCreatedEvent orderCreatedEvent);
}

public class OrderReceivedService : IOrderReceivedService
{
    private readonly ShipmentsContext _context;

    public OrderReceivedService(ShipmentsContext context)
    {
        _context = context;
    }

    public async Task HandleReceivedOrder(OrderCreatedEvent orderCreatedEvent)
    {
        _context.Add(new Shipment
        {
            CustomerId = orderCreatedEvent.CustomerId,
            OrderDate = orderCreatedEvent.OrderDate,
            PurchaseOrderKey = orderCreatedEvent.OrderKey
        });

        await _context.SaveChangesAsync();
    }
}

