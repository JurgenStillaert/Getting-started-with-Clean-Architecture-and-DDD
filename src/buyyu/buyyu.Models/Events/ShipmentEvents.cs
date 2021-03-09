using buyyu.DDD;
using System;

namespace buyyu.Models.Events
{
	public static class ShipmentEvents
	{
		public static class v1
		{
			public sealed class OrderShipped : IDomainEvent
			{
				public OrderShipped(
					Guid orderId,
					DateTime shipmentDate)
				{
					OrderId = orderId;
					ShipmentDate = shipmentDate;
				}

				public Guid OrderId { get; }
				public DateTime ShipmentDate { get; }
			}
		}
	}
}