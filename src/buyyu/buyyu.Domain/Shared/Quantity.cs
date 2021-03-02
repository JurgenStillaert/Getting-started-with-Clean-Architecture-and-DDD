using buyyu.DDD;
using System;

namespace buyyu.Domain.Shared
{
	public class Quantity : Value<Quantity>
	{
		public int Value { get; private set; }

		//Satisfy EF Core
		public Quantity() { }

		public Quantity(int qty)
		{
			if (qty < 0)
			{
				throw new ArgumentNullException(nameof(qty), "Quantity cannot be negative");
			}

			Value = qty;
		}

		public static Quantity FromInt(int qty) => new Quantity(qty);
		public static Quantity Empty() => new Quantity(0);

		public static implicit operator int(Quantity quantity) => quantity.Value;
	}
}