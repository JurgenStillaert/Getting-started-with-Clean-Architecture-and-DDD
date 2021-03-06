using buyyu.Domain.Shared;
using NUnit.Framework;
using System;

namespace buyyu.Tests
{
	internal class MoneyTests
	{
		public void CreateMoney()
		{
			//Arrange
			var amount = 100m;
			var currency = "EUR";

			//Act
			var money = Money.FromDecimalAndCurrency(amount, currency);

			//Assert
			Assert.That(money.Amount, Is.EqualTo(amount));
			Assert.That(money.Currency, Is.EqualTo(currency));
		}

		public void NoEmptyCurrencyAllowed()
		{
			Assert.Throws<ArgumentNullException>(() => Money.FromDecimalAndCurrency(0m, ""));
		}
	}
}