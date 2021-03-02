using buyyu.DDD;

namespace buyyu.Domain.Order
{
	public class OrderState : Value<OrderState>
	{
		public OrderStateEnum Value { get; private set; }

		//Satisfy EF Core
		private OrderState()
		{
		}

		private OrderState(OrderStateEnum orderStateEnum)
		{
			Value = orderStateEnum;
		}

		public static OrderState FromEnum(OrderStateEnum orderStateEnum) => new OrderState(orderStateEnum);

		public static implicit operator OrderStateEnum(OrderState orderState) => orderState.Value;

		public bool IsNewState() => OrderStateEnum.NEW == Value;

		public bool IsConfirmedState() => OrderStateEnum.CNF == Value;

		public bool IsShippedState() => OrderStateEnum.SHP == Value;

		public override string ToString()
		{
			return Value.ToString("G");
		}

		public enum OrderStateEnum
		{
			NEW,
			CNF,
			SHP
		}
	}
}