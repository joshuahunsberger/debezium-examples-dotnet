namespace ShipmentService.Data;

public class OrderLineItem
{
    public long OrderLineItemId { get; set; }

    public int Quantity { get; set; }

    public decimal? TotalPrice { get; set; }

    public long? OrderId { get; set; }

    public string? Item { get; set; }

    public string? Status { get; set; }

    public Guid OrderLineItemKey { get; set; }
}
