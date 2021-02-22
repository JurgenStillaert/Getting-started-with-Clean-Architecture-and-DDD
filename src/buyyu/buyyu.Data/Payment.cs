using System;

namespace buyyu.Data
{
	public class Payment
	{
		public Guid Id { get; set; }
		public decimal PaidAmount { get; set; }
		public DateTime PayDate { get; set; }
		public Guid OrderId { get; set; }

		public Order Order { get; set; }
	}
}