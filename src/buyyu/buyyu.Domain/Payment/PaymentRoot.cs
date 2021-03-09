using buyyu.DDD;
using buyyu.Domain.Shared;
using System;
using static buyyu.Models.Events.PaymentEvents;

namespace buyyu.Domain.Payment
{
	public class PaymentRoot : AggregateRoot<PaymentId>
	{
		public Money PaidAmount { get; private set; }
		public DateTime PayDate { get; private set; }
		public OrderId OrderId { get; private set; }

		public static PaymentRoot Create(PaymentId id, OrderId orderId, Money paidAmount)
		{
			var payment = new PaymentRoot();

			payment.Apply(new v1.PaymentCreated(id.Value, orderId, paidAmount.Amount, paidAmount.Currency, DateTime.Now));

			return payment;
		}

		protected override void EnsureValidState()
		{
			if (PaidAmount == null || PayDate == null || OrderId == null)
			{
				throw new AggregateRootInvalidStateException();
			}
		}

		private void Handle(v1.PaymentCreated @event)
		{
			Id = PaymentId.FromGuid(@event.PaymentId);
			OrderId = OrderId.FromGuid(@event.OrderId);
			PaidAmount = Money.FromDecimalAndCurrency(@event.PaidAmount, @event.Currency);
			PayDate = @event.PayDate;
		}
	}
}