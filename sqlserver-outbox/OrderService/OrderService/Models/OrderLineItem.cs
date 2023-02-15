using System;
namespace OrderService.Models;

public record OrderLineItem(string Item, int Quantity, decimal TotalPrice);

