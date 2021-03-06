using buyyu.DDD;
using System;

namespace buyyu.Domain.Payment
{
	public class PaymentId : Value<PaymentId>
	{
		public Guid Value { get; private set; }

		//Satisfy EF Core
		private PaymentId() { }

		private PaymentId(Guid paymentId)
		{
			if (paymentId == null || paymentId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(paymentId), "PaymentId cannot be empty");
			}

			Value = paymentId;
		}

		public static PaymentId FromGuid(Guid paymentId) => new PaymentId(paymentId);
		public static PaymentId FromString(string paymentId) => new PaymentId(Guid.Parse(paymentId));

		public static PaymentId CreateNew() => new PaymentId(Guid.NewGuid());
	}
}