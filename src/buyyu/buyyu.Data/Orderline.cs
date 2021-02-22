using System;

namespace buyyu.Data
{
	public class Orderline
	{
		public Guid Id { get; set; }
		public Guid OrderId { get; set; }
		public Guid ProductId { get; set; }
		public decimal Price { get; set; }
		public int Qty { get; set; }

		public Order Order { get; set; }
		public Product Product { get; set; }
	}
}