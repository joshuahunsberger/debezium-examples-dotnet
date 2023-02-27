using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderService.Data;

public partial class Customer
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;

    internal class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers", "Inventory");
            builder.HasKey(e => e.CustomerId);
            builder.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            builder.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            builder.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
        }
    }
}
