using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderService.Data;

public class OrderLineItem
{
    public long OrderLineItemId { get; set; }
    public int Quantity { get; set; }
    public decimal? TotalPrice { get; set; }
    public long OrderId { get; set; }
    public string? Item { get; set; }
    public string? Status { get; set; }
    public Guid OrderLineItemKey { get; set; }

    internal class OrderLineItemConfiguration : IEntityTypeConfiguration<OrderLineItem>
    {
        public void Configure(EntityTypeBuilder<OrderLineItem> builder)
        {
            builder.ToTable("OrderLineItems", "Inventory");

            builder.HasKey(o => o.OrderLineItemId);

            builder.Property(o => o.Quantity).IsRequired();
            builder.Property(o => o.TotalPrice).HasPrecision(19, 2).IsRequired(false);
            builder.Property(o => o.OrderId);
            builder.Property(o => o.Item).HasMaxLength(255).IsRequired(false);
            builder.Property(o => o.Status).HasMaxLength(255).IsRequired(false);
            builder.Property(o => o.OrderLineItemKey).IsRequired();
        }
    }
}

