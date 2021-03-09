using buyyu.DDD;
using System;

namespace buyyu.Models.Events
{
	public static class PaymentEvents
	{
		public static class v1
		{
			public sealed class PaymentCreated : IDomainEvent
			{
				public PaymentCreated(
					Guid paymentId,
					Guid orderId,
					decimal paidAmount,
					string currency,
					DateTime payDate)
				{
					PaymentId = paymentId;
					OrderId = orderId;
					PaidAmount = paidAmount;
					Currency = currency;
					PayDate = payDate;
				}

				public Guid PaymentId { get; }
				public Guid OrderId { get; }
				public decimal PaidAmount { get; }
				public string Currency { get; }
				public DateTime PayDate { get; }
			}
		}
	}
}