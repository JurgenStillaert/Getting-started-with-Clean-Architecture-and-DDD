using buyyu.DDD;
using System;

namespace buyyu.Domain.Order
{
	public class OrderlineId : Value<OrderlineId>
	{
		public Guid Value { get; private set; }

		//Satisfy EF Core
		private OrderlineId() { }

		private OrderlineId(Guid orderlineId)
		{
			if (orderlineId == null || orderlineId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(orderlineId), "OrderlineId cannot be empty");
			}

			Value = orderlineId;
		}

		public static OrderlineId GenerateNew() => new OrderlineId(Guid.NewGuid());

		public static OrderlineId FromGuid(Guid orderlineId) => new OrderlineId(orderlineId);

		public static OrderlineId FromString(string orderlineId) => new OrderlineId(Guid.Parse(orderlineId));

		public static implicit operator Guid(OrderlineId orderlineId) => orderlineId.Value;
	}
}