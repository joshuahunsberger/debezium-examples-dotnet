namespace ShipmentService.Data;

public class PurchaseOrder
{
    public long PurchaseOrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public Guid PurchaseOrderKey { get; set; }
}
