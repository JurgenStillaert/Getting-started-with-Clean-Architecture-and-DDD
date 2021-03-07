using System;
using System.Collections.Generic;

namespace buyyu.Models.Dtos
{
	public class OrderDto
	{
		public Guid OrderId { get; set; }
		public Guid ClientId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal PaidAmount { get; set; }
		public string State { get; set; }
		public List<OrderlineDto> Orderlines { get; set; }

		public class OrderlineDto
		{
			public Guid OrderlineId { get; set; }
			public Guid ProductId { get; set; }
			public decimal Price { get; set; }
			public int Qty { get; set; }
		}
	}
}