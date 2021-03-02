using buyyu.DDD;
using System;

namespace buyyu.Domain.Order
{
	public class OrderDate : Value<OrderDate>
	{
		public DateTime Value { get; private set; }

		//Satisfy EF Core
		public OrderDate() { }

		public OrderDate(DateTime dateTime)
		{
			if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
			{
				throw new ArgumentNullException(nameof(dateTime), "OrderDate must have a valid date");
			}

			Value = dateTime;
		}

		public static OrderDate FromDateTime(DateTime orderDate) => new OrderDate(orderDate);
		public static OrderDate Now() => new OrderDate(DateTime.Now);

		public static implicit operator DateTime(OrderDate orderDate) => orderDate.Value;
	}
}