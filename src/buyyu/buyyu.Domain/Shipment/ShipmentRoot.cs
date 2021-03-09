using buyyu.DDD;
using buyyu.Domain.Shared;
using static buyyu.Models.Events.ShipmentEvents;

namespace buyyu.Domain.Shipment
{
	public class ShipmentRoot : AggregateRoot<OrderId>
	{
		public ShipmentDate ShipmentDate { get; private set; }

		public static ShipmentRoot Ship(OrderId orderId)
		{
			var shipment = new ShipmentRoot();

			shipment.Apply(new v1.OrderShipped(orderId, ShipmentDate.Now()));

			return shipment;
		}

		protected override void EnsureValidState()
		{
		}

		private void Handle(v1.OrderShipped @event)
		{
			Id = OrderId.FromGuid(@event.OrderId);
			ShipmentDate = ShipmentDate.FromDateTime(@event.ShipmentDate);
		}
	}
}