using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderService.Data;

public class PurchaseOrder
{
    public long PurchaseOrderId { get; set; }
    public long CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public Guid PurchaseOrderKey { get; set; }

    public List<OrderLineItem> OrderLineItems { get; set; } = new List<OrderLineItem>();

    public decimal OrderTotal => OrderLineItems.Sum(o => o.TotalPrice ?? 0);

    internal class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.ToTable("PurchaseOrders", "Inventory");

            builder.HasKey(p => p.PurchaseOrderId);
            builder.Property(p => p.CustomerId).IsRequired();
            builder.Property(p => p.OrderDate).IsRequired();
            builder.Property(p => p.PurchaseOrderKey).IsRequired();

            builder.HasMany(p => p.OrderLineItems).WithOne().HasForeignKey(o => o.OrderId);
        }
    }
}

