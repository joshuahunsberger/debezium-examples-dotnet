using System;
using OrderService.Data;

namespace OrderService.Events;

public class InvoiceCreatedEvent
{
    public InvoiceCreatedEvent(PurchaseOrder purchaseOrder)
    {
        OrderKey = purchaseOrder.PurchaseOrderKey;
        CustomerId = purchaseOrder.CustomerId;
        OrderDate = purchaseOrder.OrderDate;
        InvoiceValue = purchaseOrder.OrderTotal;
    }

    public Guid OrderKey { get; set; }
    public long CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal InvoiceValue { get; set; }
}
