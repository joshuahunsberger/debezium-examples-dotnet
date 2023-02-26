using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Services;

namespace OrderService.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderServices(this IServiceCollection services, IConfiguration configuration)
        {
            var ordersConnectionString = configuration.GetConnectionString("Orders");
            return services
                .AddDbContext<OrdersContext>(options => options.UseSqlServer(ordersConnectionString))
                .AddScoped<IOrderCreationService, OrderCreationService>();
        }
    }
}

