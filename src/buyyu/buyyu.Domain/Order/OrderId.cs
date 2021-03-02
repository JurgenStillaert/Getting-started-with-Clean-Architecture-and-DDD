using buyyu.DDD;
using System;

namespace buyyu.Domain.Order
{
	public class OrderId : Value<OrderId>
	{
		public Guid Value { get; private set; }

		//Satisfy EF Core
		private OrderId() { }

		private OrderId(Guid orderId)
		{
			if (orderId == null || orderId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(orderId), "OrderId cannot be empty");
			}

			Value = orderId;
		}

		public static OrderId FromGuid(Guid orderId) => new OrderId(orderId);
		public static OrderId FromString(string orderId) => new OrderId(Guid.Parse(orderId));

		public static implicit operator Guid(OrderId orderId) => orderId.Value;
	}
}