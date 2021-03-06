using buyyu.DDD;
using buyyu.Domain.Shared;
using System;

namespace buyyu.Domain.Payment
{
	public class PaymentRoot : AggregateRoot<PaymentId>
	{
		public Money PaidAmount { get; private set; }
		public DateTime PayDate { get; private set; }
		public OrderId OrderId { get; private set; }

		public static PaymentRoot Create(PaymentId id, OrderId orderId, Money paidAmount)
		{
			var payment = new PaymentRoot
			{
				Id = id,
				OrderId = orderId,
				PaidAmount = paidAmount,
				PayDate = DateTime.Now
			};

			payment.EnsureValidState();

			return payment;
		}

		protected override void EnsureValidState()
		{
			if (PaidAmount == null || PayDate == null || OrderId == null)
			{
				throw new AggregateRootInvalidStateException();
			}
		}
	}
}