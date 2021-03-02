using buyyu.DDD;
using buyyu.Domain.Order;
using System;

namespace buyyu.Domain.Payment
{
	public class PaymentRoot : AggregateRoot<PaymentId>
	{
		public decimal PaidAmount { get; private set; }
		public DateTime PayDate { get; private set; }
		public Guid OrderId { get; private set; }

		public OrderRoot Order { get; private set; }

		public static PaymentRoot Create(decimal paidAmount)
		{
			return new PaymentRoot
			{
				PaidAmount = paidAmount,
				PayDate = DateTime.Now
			};
		}

		protected override void EnsureValidation()
		{
			throw new NotImplementedException();
		}
	}
}