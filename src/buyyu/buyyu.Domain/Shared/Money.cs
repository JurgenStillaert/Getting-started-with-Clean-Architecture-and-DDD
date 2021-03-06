using buyyu.DDD;
using System;

namespace buyyu.Domain.Shared
{
	public class Money : Value<Money>
	{
		public decimal Amount { get; private set; }
		public string Currency { get; private set; }

		//Satisfy EF Core
		private Money() { }

		private Money(decimal amount, string currency)
		{
			if (string.IsNullOrEmpty(currency))
			{
				throw new ArgumentNullException($"'{nameof(currency)}' cannot be null or empty", nameof(currency));
			}

			Amount = amount;
			Currency = currency;
		}

		public static Money FromDecimalAndCurrency(decimal amount, string currency) => new Money(amount, currency);

		public static Money Empty(string currency) => new Money(0, currency);

		public override string ToString()
		{
			return ToString("post");
		}

		private string ToString(string format)
		{
			var strAmount = Amount.ToString("0.##");
			if (format.ToLowerInvariant() == "pre")
			{
				return $"{Currency} {strAmount}";
			}
			else
			{
				return $"{strAmount} {Currency}";
			}
		}

		public static Money operator +(Money mny1, Money mny2)
		{
			if (mny1.Currency != mny2.Currency)
			{
				throw new InvalidOperationException("Cannot add money with different currencies");
			}

			return new Money(mny1.Amount + mny2.Amount, mny1.Currency);
		}
	}
}