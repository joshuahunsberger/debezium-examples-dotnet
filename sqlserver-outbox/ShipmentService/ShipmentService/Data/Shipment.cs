﻿namespace ShipmentService.Data;

public class Shipment
{
    public long ShipmentId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public Guid PurchaseOrderKey { get; set; }
}
