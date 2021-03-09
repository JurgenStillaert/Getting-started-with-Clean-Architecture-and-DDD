using buyyu.BL.Commands;
using buyyu.Data.Repositories.Interfaces;
using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Product;
using buyyu.Domain.Shared;
using buyyu.Models.Commands;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace buyyu.Tests
{
	internal class CreateOrderCommandHandlerTests
	{
		[Test]
		public async Task CreateNormalOrder()
		{
			//Arrange
			var orderId = OrderId.FromGuid(Guid.NewGuid());
			var clientId = ClientId.FromGuid(Guid.NewGuid());
			var productId = ProductId.FromGuid(Guid.NewGuid());
			var qty = Quantity.FromInt(10);
			var orderlines = new List<CreateOrderCommand.OrderLineCommand> { new CreateOrderCommand.OrderLineCommand(productId, qty) };
			var command = new CreateOrderCommand(orderId, clientId, orderlines);
			var mockOrderRepo = new Mock<IRepository<OrderRoot, OrderId>>();
			var mockProductRepo = new Mock<IProductRepository>();
			var mockMediator = new Mock<IMediator>();

			mockProductRepo.Setup(x => x.GetProduct(productId)).ReturnsAsync(
				new ProductRoot(
					productId,
					ProductName.FromString("Dummy"),
					Description.FromString("Lorem ipsum dolor sit amet, consectetur adipiscing elit"),
					Money.FromDecimalAndCurrency(10, "EUR")));

			var sut = new CreateOrderCommandHandler(mockProductRepo.Object, mockOrderRepo.Object, mockMediator.Object);

			//Act
			await sut.Handle(command, CancellationToken.None);

			//Assert
			mockOrderRepo.Verify(x => x.Add(It.Is<OrderRoot>(
				or =>
				or.ClientId == clientId
				&& or.Id == orderId
				&& or.Lines.Count == 1
				&& or.Lines[0].Price.Amount == 10
				&& or.Lines[0].ProductId == productId
				&& or.Lines[0].Qty == 10
				&& or.PaidAmount.Amount == 0m
				&& or.State == OrderState.OrderStateEnum.NEW
				&& or.TotalAmount.Amount == 100m)));
		}
	}
}