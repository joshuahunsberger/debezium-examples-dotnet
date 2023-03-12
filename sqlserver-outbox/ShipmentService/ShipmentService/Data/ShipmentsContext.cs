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

    public virtual DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.ShipmentId).HasName("PK__Shipment__5CAD37ED22C6B818");

            entity.ToTable("Shipment", "Inventory");
        });
    }
}
