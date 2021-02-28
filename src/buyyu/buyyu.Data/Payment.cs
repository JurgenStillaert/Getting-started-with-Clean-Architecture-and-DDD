using System;

namespace buyyu.Data
{
	public class Payment
	{
		public Guid Id { get; private set; }
		public decimal PaidAmount { get; private set; }
		public DateTime PayDate { get; private set; }
		public Guid OrderId { get; private set; }

		public Order Order { get; private set; }

		public static Payment Create(decimal paidAmount)
		{
			return new Payment
			{
				PaidAmount = paidAmount,
				PayDate = DateTime.Now
			};
		}
	}
}