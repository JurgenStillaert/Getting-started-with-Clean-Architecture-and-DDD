using buyyu.BL;
using buyyu.BL.Interfaces;
using buyyu.Data.Repositories.Interfaces;
using buyyu.Domain.Order;
using buyyu.Domain.Product;
using buyyu.Domain.Shared;
using buyyu.Tests.Builders;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static buyyu.Models.Dtos.OrderDto;

namespace buyyu.Tests
{
	public class OrderServiceTests
	{
		private Mock<IOrderRepository> mockOrderRepository;
		private Mock<IProductRepository> mockProductRepository;
		private Mock<IMailService> mockMailService;

		[SetUp]
		public void Setup()
		{
			mockOrderRepository = new Mock<IOrderRepository>();
			mockProductRepository = new Mock<IProductRepository>();
			mockMailService = new Mock<IMailService>();

			mockProductRepository.Setup(x => x.GetProduct(Guid.Parse("de679c55-4c13-4fe7-91b4-69cbce3223a2"))).ReturnsAsync(new ProductRoot(
				ProductId.FromString("de679c55-4c13-4fe7-91b4-69cbce3223a2"),
				ProductName.FromString("Office Chair Beta"),
				Description.FromString("Implement an ergonomic seating solution for your office with this maroon multipurpose chair. The included tilt tension knob lets you calibrate the tilt and recline resistance to your desired configuration, while the adjustable seat and armrests optimize your seating position for correct posture."),
				Money.FromDecimalAndCurrency(169, "EUR")
			));

			mockProductRepository.Setup(x => x.GetProduct(Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"))).ReturnsAsync(new ProductRoot(
				ProductId.FromString("32f75bce-16a0-4070-9fac-4289678c191f"),
				ProductName.FromString("Office Chair Manager"),
				Description.FromString("The Lockland Big & Tall bonded leather managers chair offers top quality comfort, multiple adjustment features."),
				Money.FromDecimalAndCurrency(263, "EUR")
			));

			mockProductRepository.Setup(x => x.GetProduct(Guid.Parse("bcbc1851-6317-4022-be62-53d29c04bcda"))).ReturnsAsync(new ProductRoot(
				ProductId.FromString("bcbc1851-6317-4022-be62-53d29c04bcda"),
				ProductName.FromString("Vintage Desk"),
				Description.FromString("Carve out a personal workspace with this storage desk. The simple design and classic mid-century modern details make this desk perfect for modern decor themes or casual open office settings, and the rectangular desktop provides space for a laptop and peripherals."),
				Money.FromDecimalAndCurrency(305, "EUR")
			));

			mockProductRepository.Setup(x => x.GetProduct(Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"))).ReturnsAsync(new ProductRoot(
				ProductId.FromString("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
				ProductName.FromString("Desk Techni"),
				Description.FromString("The Techni Mobili Complete Workstation Desk is everything you need in a computer desk and stay organized."),
				Money.FromDecimalAndCurrency(295, "EUR")
			));
		}

		[Test]
		public async Task GetOrder_ValidId_OrderDtoReturned()
		{
			//Arrange
			var orderId = Guid.Parse("98c981ce-8a7a-4aa5-ad78-204564ad0c5d");

			var sut = new OrderService(
				mockOrderRepository.Object,
				mockProductRepository.Object,
				mockMailService.Object
				);

			//Act
			await sut.GetOrder(orderId);

			//Assert
			mockOrderRepository.Verify(x => x.GetOrderDto(orderId), Times.Once);
		}

		[Test]
		public async Task CreateOrder_OrderWithTwoProducts_OrderSaved()
		{
			//Arrange
			var inDto = new Models.Dtos.OrderDto
			{
				ClientId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OrderDate = DateTime.MinValue,
				OrderId = Guid.Parse("c4e8f239-f93d-4cf8-9eb9-7871fa5f79e9"),
				Orderlines = new List<OrderlineDto>
				{
					new OrderlineDto
					{
						ProductId = Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
						Qty = 10
					},
					new OrderlineDto
					{
						ProductId = Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"),
						Qty = 20
					}
				},
				TotalAmount = 0m
			};

			var outDto = new Models.Dtos.OrderDto
			{
				ClientId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OrderId = Guid.Parse("c4e8f239-f93d-4cf8-9eb9-7871fa5f79e9"),
				Orderlines = new List<OrderlineDto>
				{
					new OrderlineDto
					{
						OrderlineId = Guid.NewGuid(),
						Price = 295.00m,
						ProductId = Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
						Qty = 10
					},
					new OrderlineDto
					{
						OrderlineId = Guid.NewGuid(),
						Price = 263.00m,
						ProductId = Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"),
						Qty = 20
					}
				},
				PaidAmount = 0m,
				State = "NEW",
				TotalAmount = 8210.00m
			};

			mockOrderRepository.Setup(x => x.GetOrderDto(It.IsAny<Guid>())).ReturnsAsync(outDto);

			var sut = new OrderService(
				mockOrderRepository.Object,
				mockProductRepository.Object,
				mockMailService.Object
				);

			//Act
			var returnedDto = await sut.CreateOrder(inDto);

			//Assert
			mockOrderRepository.Verify(x =>
				x.AddSave(It.Is<OrderRoot>(x =>
					x.ClientId == Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
					&& x.Id == inDto.OrderId
					&& x.State == OrderState.OrderStateEnum.NEW
					&& x.PaidAmount.Amount == 0m
					&& x.TotalAmount.Amount == 8210.00m
					&& x.Lines.Count == 2
					&& x.Lines[0].ProductId == Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e")
					&& x.Lines[0].Price.Amount == 295.00m
					&& x.Lines[0].Qty == 10
					&& x.Lines[1].ProductId == Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f")
					&& x.Lines[1].Price.Amount == 263.00m
					&& x.Lines[1].Qty == 20
				))
			);

			Assert.That(returnedDto, Is.EqualTo(outDto));
		}

		[Test]
		public async Task UpdateOrder_OrderWithTwoProducts_OrderUpdated()
		{
			//Arrange
			var inDto = new Models.Dtos.OrderDto
			{
				OrderId = Guid.Parse("3ed7fa03-fe11-441b-b59d-efab4928a6b9"),
				Orderlines = new List<OrderlineDto>
				{
					new OrderlineDto
					{
						OrderlineId = Guid.Parse("fc0e1862-21c3-4cd0-ae9c-e02986b8f283"),
						ProductId = Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
						Qty = 10
					},
					new OrderlineDto
					{
						OrderlineId = Guid.Parse("3510659d-d3c0-43bf-aca5-d2bbede87685"),
						ProductId = Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"),
						Qty = 30
					},
					new OrderlineDto
					{
						ProductId = Guid.Parse("bcbc1851-6317-4022-be62-53d29c04bcda"),
						Qty = 40
					}
				}
			};

			var outDto = new Models.Dtos.OrderDto
			{
				ClientId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OrderId = Guid.Parse("5967544b-31b2-48ce-a746-2b6db6ff187a"),
				Orderlines = new List<OrderlineDto>
				{
					new OrderlineDto
					{
						OrderlineId = Guid.Parse("fc0e1862-21c3-4cd0-ae9c-e02986b8f283"),
						Price = 295.00m,
						ProductId = Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
						Qty = 10
					},
					new OrderlineDto
					{
						OrderlineId = Guid.Parse("3510659d-d3c0-43bf-aca5-d2bbede87685"),
						Price = 263.00m,
						ProductId = Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"),
						Qty = 30
					},
					new OrderlineDto
					{
						OrderlineId = Guid.Parse("bcbc1851-6317-4022-be62-53d29c04bcda"),
						Price = 305.00m,
						ProductId = Guid.Parse("bcbc1851-6317-4022-be62-53d29c04bcda"),
						Qty = 40
					}
				},
				PaidAmount = 0m,
				State = "NEW",
				TotalAmount = 23040.00m
			};

			var newOrder = OrderBuilder.NewOrderWithTwoProducts(inDto.OrderId);

			mockOrderRepository.Setup(x => x.GetOrderDto(It.IsAny<Guid>())).ReturnsAsync(outDto);
			mockOrderRepository.Setup(x => x.GetOrder(newOrder.Id)).ReturnsAsync(newOrder);

			var sut = new OrderService(
				mockOrderRepository.Object,
				mockProductRepository.Object,
				mockMailService.Object
				);

			//Act
			var returnedDto = await sut.UpdateOrder(inDto.OrderId, inDto);

			//Assert
			mockOrderRepository.Verify(x =>
				x.Save(It.Is<OrderRoot>(x =>
					x.ClientId == Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
					&& x.Id == Guid.Parse("3ed7fa03-fe11-441b-b59d-efab4928a6b9")
					&& x.State == OrderState.OrderStateEnum.NEW
					&& x.PaidAmount.Amount == 0m
					&& x.TotalAmount.Amount == 23040.00m
					&& x.Lines.Count == 3
					&& x.Lines[0].ProductId == Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e")
					&& x.Lines[0].Price.Amount == 295.00m
					&& x.Lines[0].Qty == 10
					&& x.Lines[1].ProductId == Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f")
					&& x.Lines[1].Price.Amount == 263.00m
					&& x.Lines[1].Qty == 30
					&& x.Lines[2].ProductId == Guid.Parse("bcbc1851-6317-4022-be62-53d29c04bcda")
					&& x.Lines[2].Price.Amount == 305.00m
					&& x.Lines[2].Qty == 40
				))
			);

			Assert.That(returnedDto, Is.EqualTo(outDto));
		}
	}
}