using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderService.Data;

public class OutboxEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string AggregateType { get; set; } = null!;
    public string AggregateId { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? Payload { get; set; }

    internal class OutboxEventEntityTypeConfiguration : IEntityTypeConfiguration<OutboxEvent>
    {
        public void Configure(EntityTypeBuilder<OutboxEvent> builder)
        {
            builder.ToTable("OutboxEvents", "Inventory");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.AggregateId).HasMaxLength(255);
            builder.Property(e => e.AggregateType).HasMaxLength(255);
            builder.Property(e => e.Payload).HasMaxLength(4000);
            builder.Property(e => e.Type).HasMaxLength(255);
        }
    }
}
