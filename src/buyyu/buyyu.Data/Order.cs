using System;
using System.Collections.Generic;

namespace buyyu.Data
{
	public class Order
	{
		public Guid Id { get; set; }
		public Guid ClientId { get; set; }
		public Guid OrderStateId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal PaidAmount { get; set; }

		public OrderState State { get; set; }
		public List<Orderline> Lines { get; set; }
		public List<Payment> Payments { get; set; }
	}
}