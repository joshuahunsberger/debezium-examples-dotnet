using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShipmentService.Data;

public class ShipmentsContext : DbContext
{
    public ShipmentsContext(DbContextOptions<ShipmentsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<OrderLineItem> OrderLineItems { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8446C1199");

            entity.ToTable("Customers", "Inventory");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D105348FB6B3C7").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrderLineItem>(entity =>
        {
            entity.HasKey(e => e.OrderLineItemId).HasName("PK__OrderLin__6E480521E9BE89E4");

            entity.ToTable("OrderLineItems", "Inventory");

            entity.Property(e => e.Item).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(19, 2)");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderId).HasName("PK__Purchase__036BAC44F7D98B34");

            entity.ToTable("PurchaseOrders", "Inventory");

            entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrderID");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.ShipmentId).HasName("PK__Shipment__5CAD37ED22C6B818");

            entity.ToTable("Shipment", "Inventory");
        });
    }
}
