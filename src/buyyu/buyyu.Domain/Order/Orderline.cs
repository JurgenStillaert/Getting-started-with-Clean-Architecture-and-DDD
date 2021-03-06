using buyyu.DDD;
using buyyu.Domain.Shared;

namespace buyyu.Domain.Order
{
	public class Orderline : Entity<OrderlineId>
	{
		public ProductId ProductId { get; private set; }
		public Money Price { get; private set; }
		public Quantity Qty { get; private set; }

		public OrderRoot Order { get; private set; }

		public static Orderline Create(OrderlineId orderlineId, ProductId productId, Money price, Quantity qty)
		{
			var orderline = new Orderline { Id = orderlineId, Price = price, ProductId = productId, Qty = qty };

			return orderline;
		}

		public void Update(Money price, Quantity qty)
		{
			Price = price;
			Qty = qty;
		}
	}
}