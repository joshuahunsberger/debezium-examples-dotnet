using Microsoft.EntityFrameworkCore;

namespace OrderService.Data;

public class OrdersContext : DbContext
{
    public OrdersContext(DbContextOptions<OrdersContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderLineItem).Assembly);
    }
}

