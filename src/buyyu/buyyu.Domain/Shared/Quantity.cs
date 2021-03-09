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

		public static Quantity operator +(Quantity qty1, Quantity qty2)
		{
			return Quantity.FromInt(qty1.Value + qty2.Value);
		}

		public static Quantity operator -(Quantity qty1, Quantity qty2)
		{
			if (qty1.Value - qty2.Value < 0)
			{
				throw new InvalidOperationException("Quantity can not be lower than 0");
			}

			return Quantity.FromInt(qty1.Value - qty2.Value);
		}
	}
}