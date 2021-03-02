using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using System;

namespace buyyu.Tests.Builders
{
	public class OrderBuilder
	{
		public static OrderRoot NewOrderWithTwoProducts(Guid orderId)
		{
			var newOrder = OrderRoot.Create(OrderId.FromGuid(orderId), ClientId.FromString("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
			newOrder.AddOrderline(ProductId.FromString("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"), Money.FromDecimalAndCurrency(295.00m, "EUR"), Quantity.FromInt(10));
			newOrder.AddOrderline(ProductId.FromString("32f75bce-16a0-4070-9fac-4289678c191f"), Money.FromDecimalAndCurrency(263.00m, "EUR"), Quantity.FromInt(20));

			return newOrder;
		}
	}
}