namespace ShipmentService.Events;

public class OrderCreatedEvent
{
    public Guid OrderKey { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }

    public List<LineItem> LineItems { get; set; } = new List<LineItem>();
}

public class LineItem
{
    public Guid OrderLineItemKey { get; set; }
    public int Quantity { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? Item { get; set; }
    public string? Status { get; set; }
}

