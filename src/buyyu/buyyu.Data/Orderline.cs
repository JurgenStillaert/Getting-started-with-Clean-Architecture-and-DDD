using System;

namespace buyyu.Data
{
	public class Orderline
	{
		public Guid Id { get; private set; }
		public Guid OrderId { get; private set; }
		public Guid ProductId { get; private set; }
		public decimal Price { get; private set; }
		public int Qty { get; private set; }

		public Order Order { get; private set; }
		public Product Product { get; private set; }

		public static Orderline Create(Guid productId, decimal price, int qty)
		{
			if (productId == null || productId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(productId), "ProductId cannot be empty");
			}

			if (qty <= 0)
			{
				throw new ArgumentNullException(nameof(qty), "Qty must be a positive integer");
			}

			var orderline = new Orderline { Price = price, ProductId = productId, Qty = qty };

			return orderline;
		}

		public void Update(decimal price, int qty)
		{
			if (qty <= 0)
			{
				throw new ArgumentNullException(nameof(qty), "Qty must be a positive integer");
			}

			Price = price;
			Qty = qty;
		}
	}
}