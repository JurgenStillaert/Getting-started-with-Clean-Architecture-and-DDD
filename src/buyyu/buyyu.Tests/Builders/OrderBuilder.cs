using buyyu.Data;
using System;
using System.Collections.Generic;

namespace buyyu.Tests.Builders
{
	public class OrderBuilder
	{
		//Not used, only here as an example
		public static Order NewOrderWithTwoProductsUsingReflection(Guid orderId, OrderState newOrderState)
		{
			var orderType = typeof(Order);
			var newOrder = (Order)Activator.CreateInstance(orderType);
			typeof(Order).GetProperty(nameof(newOrder.ClientId)).SetValue(newOrder, Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
			typeof(Order).GetProperty(nameof(newOrder.Id)).SetValue(newOrder, orderId);
			typeof(Order).GetProperty(nameof(newOrder.OrderDate)).SetValue(newOrder, new DateTime(2021, 2, 19, 17, 26, 57));
			typeof(Order).GetProperty(nameof(newOrder.OrderStateId)).SetValue(newOrder, newOrderState.Id);
			typeof(Order).GetProperty(nameof(newOrder.PaidAmount)).SetValue(newOrder, 0m);
			typeof(Order).GetProperty(nameof(newOrder.Payments)).SetValue(newOrder, new List<Payment>());
			typeof(Order).GetProperty(nameof(newOrder.State)).SetValue(newOrder, newOrderState);
			typeof(Order).GetProperty(nameof(newOrder.TotalAmount)).SetValue(newOrder, 23040.00m);

			var orderlineType = typeof(Orderline);
			var orderline1 = (Orderline)Activator.CreateInstance(orderlineType);
			typeof(Orderline).GetProperty(nameof(orderline1.Id)).SetValue(orderline1, Guid.Parse("fc0e1862-21c3-4cd0-ae9c-e02986b8f283"));
			typeof(Orderline).GetProperty(nameof(orderline1.Price)).SetValue(orderline1, 295.00m);
			typeof(Orderline).GetProperty(nameof(orderline1.ProductId)).SetValue(orderline1, Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"));
			typeof(Orderline).GetProperty(nameof(orderline1.Qty)).SetValue(orderline1, 10);

			var orderline2 = (Orderline)Activator.CreateInstance(orderlineType);
			typeof(Orderline).GetProperty(nameof(orderline2.Id)).SetValue(orderline2, Guid.Parse("3510659d-d3c0-43bf-aca5-d2bbede87685"));
			typeof(Orderline).GetProperty(nameof(orderline2.Price)).SetValue(orderline2, 263.00m);
			typeof(Orderline).GetProperty(nameof(orderline2.ProductId)).SetValue(orderline2, Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"));
			typeof(Orderline).GetProperty(nameof(orderline2.Qty)).SetValue(orderline2, 20);

			typeof(Order).GetProperty(nameof(newOrder.Lines)).SetValue(newOrder, new List<Orderline>
			{
				orderline1,
				orderline2
			});

			return newOrder;
		}

		public static Order NewOrderWithTwoProducts(Guid orderId, OrderState newOrderState)
		{
			var newOrder = Order.Create(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), newOrderState.Id);
			newOrder.AddOrderline(Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"), 295.00m, 10);
			newOrder.AddOrderline(Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"), 263, 20);

			var t = typeof(Order);
			typeof(Order).GetProperty(nameof(newOrder.Id)).SetValue(newOrder, orderId);
			typeof(Order).GetProperty(nameof(newOrder.State)).SetValue(newOrder, newOrderState);

			return newOrder;
		}
	}
}