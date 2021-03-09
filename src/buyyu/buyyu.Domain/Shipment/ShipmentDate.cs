using buyyu.DDD;
using System;

namespace buyyu.Domain.Shipment
{
	public class ShipmentDate : Value<ShipmentDate>
	{
		public DateTime Value { get; }

		//Satisfy EF Core
		private ShipmentDate()
		{
		}

		private ShipmentDate(DateTime shipmentDate)
		{
			Value = shipmentDate;
		}

		public static ShipmentDate Now() => new ShipmentDate(DateTime.Now);

		public static ShipmentDate FromDateTime(DateTime shipmentDate) => new ShipmentDate(shipmentDate);

		public static implicit operator DateTime(ShipmentDate shipmentDate) => shipmentDate.Value;
	}
}