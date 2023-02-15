using System;
namespace OrderService.Models;

public record CreateOrderRequest(long CustomerId, DateTime OrderDate, List<OrderLineItem> LineItems);
