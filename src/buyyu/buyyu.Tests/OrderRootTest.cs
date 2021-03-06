using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using NUnit.Framework;

namespace buyyu.Tests
{
	public class OrderRootTest
	{
		[Test]
		public void CreateValidOrder()
		{
			//Arrange
			var clientId = ClientId.FromString("031c7ca8-ecab-475a-a207-ce02c7addefc");
			var orderId = OrderId.FromString("e8959fda-3d46-4d74-8f1a-58a408ccf62e");

			//Act
			var order = OrderRoot.Create(orderId, clientId);

			//Assert
			Assert.That(order.ClientId, Is.EqualTo(clientId));
			Assert.That(order.Id, Is.EqualTo(orderId));
		}

		[Test]
		public void AddOrderline()
		{
			//Arrange
			var clientId = ClientId.FromString("031c7ca8-ecab-475a-a207-ce02c7addefc");
			var orderId = OrderId.FromString("e8959fda-3d46-4d74-8f1a-58a408ccf62e");
			var order = OrderRoot.Create(orderId, clientId);

			var productId = ProductId.FromString("02ad2005-1cb8-4bcc-ae0e-4d9eefabadd8");
			var price = Money.FromDecimalAndCurrency(100m, "EUR");
			var quantity = Quantity.FromInt(5);

			//Act
			order.AddOrderline(productId, price, quantity);

			//Arrange
			Assert.That(order.Lines.Count, Is.EqualTo(1));
			Assert.That(order.Lines[0].ProductId, Is.EqualTo(productId));
			Assert.That(order.Lines[0].Price, Is.EqualTo(price));
			Assert.That(order.Lines[0].Qty, Is.EqualTo(quantity));
		}
	}
}