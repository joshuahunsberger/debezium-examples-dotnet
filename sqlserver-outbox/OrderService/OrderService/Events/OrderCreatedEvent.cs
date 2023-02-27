namespace OrderService.Events;

public class OrderCreatedEvent
{
    public OrderCreatedEvent(Data.PurchaseOrder orderEntity)
    {
        OrderKey = orderEntity.PurchaseOrderKey;
        CustomerId = orderEntity.CustomerId;
        OrderDate = orderEntity.OrderDate;
        LineItems = orderEntity.OrderLineItems
            .Select(li => new LineItem
            {
                OrderLineItemKey = li.OrderLineItemKey,
                Quantity = li.Quantity,
                TotalPrice = li.TotalPrice,
                Item = li.Item,
                Status = li.Status
            })
            .ToList();
    }

    public Guid OrderKey { get; set; }
    public long CustomerId { get; set; }
    public DateTime OrderDate { get; set; }

    public List<LineItem> LineItems { get; set; }
}

public class LineItem
{
    public Guid OrderLineItemKey { get; set; }
    public int Quantity { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? Item { get; set; }
    public string? Status { get; set; }
}
