using buyyu.DDD;
using buyyu.Domain.Shared;

namespace buyyu.Domain.Shipment
{
	public class ShipmentRoot : AggregateRoot<OrderId>
	{
		public ShipmentDate ShipmentDate { get; private set; }

		public static ShipmentRoot Ship(OrderId orderId)
		{
			return new ShipmentRoot
			{
				Id = orderId,
				ShipmentDate = ShipmentDate.Now()
			};
		}

		protected override void EnsureValidState()
		{
		}
	}
}