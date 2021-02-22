using buyyu.BL;
using buyyu.BL.Interfaces;
using buyyu.Data;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using buyyu.Data.Repositories.Interfaces;
using static buyyu.Models.OrderDto;

namespace buyyu.Tests
{
	public class OrderServiceTests
	{
		private Mock<IOrderRepository> mockOrderRepository;
		private Mock<IOrderStateRepository> mockOrderStateRepository;
		private Mock<IProductRepository> mockProductRepository;
		private Mock<IMailService> mockMailService;
		private Mock<IWarehouseService> mockWarehouseService;

		private OrderState NewOrderState = new OrderState
		{
			Id = Guid.Parse("bd8be3d2-8028-45e2-a211-bf737a2508c1"),
			Name = "Initiated",
			ShortCode = "NEW"
		};

		private OrderState ConfirmedOrderState = new OrderState
		{
			Id = Guid.Parse("82d9ce01-9f25-48b1-8af3-93f52426676f"),
			Name = "Confirmed",
			ShortCode = "CNF"
		};

		private OrderState ShippedOrderState = new OrderState
		{
			Id = Guid.Parse("4b5549bb-b1b2-4964-9818-da984baab4ff"),
			Name = "Shipped",
			ShortCode = "SHP"
		};

		[SetUp]
		public void Setup()
		{
			mockOrderRepository = new Mock<IOrderRepository>();
			mockOrderStateRepository = new Mock<IOrderStateRepository>();
			mockProductRepository = new Mock<IProductRepository>();
			mockMailService = new Mock<IMailService>();
			mockWarehouseService = new Mock<IWarehouseService>();

			mockOrderStateRepository.Setup(x => x.GetOrderStateByCode("NEW")).ReturnsAsync(NewOrderState);
			mockOrderStateRepository.Setup(x => x.GetOrderStateByCode("CNF")).ReturnsAsync(ConfirmedOrderState);
			mockOrderStateRepository.Setup(x => x.GetOrderStateByCode("SHP")).ReturnsAsync(ShippedOrderState);

			mockProductRepository.Setup(x => x.GetProduct(Guid.Parse("de679c55-4c13-4fe7-91b4-69cbce3223a2"))).ReturnsAsync(new Product
			{
				Id = Guid.Parse("de679c55-4c13-4fe7-91b4-69cbce3223a2"),
				Name = "Office Chair Beta",
				Description = "Implement an ergonomic seating solution for your office with this maroon multipurpose chair. The included tilt tension knob lets you calibrate the tilt and recline resistance to your desired configuration, while the adjustable seat and armrests optimize your seating position for correct posture.",
				Price = 169,
				QtyInStock = 213
			});

			mockProductRepository.Setup(x => x.GetProduct(Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"))).ReturnsAsync(new Product
			{
				Id = Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"),
				Name = "Office Chair Manager",
				Description = "The Lockland Big & Tall bonded leather managers chair offers top quality comfort, multiple adjustment features.",
				Price = 263,
				QtyInStock = 75
			});

			mockProductRepository.Setup(x => x.GetProduct(Guid.Parse("bcbc1851-6317-4022-be62-53d29c04bcda"))).ReturnsAsync(new Product
			{
				Id = Guid.Parse("bcbc1851-6317-4022-be62-53d29c04bcda"),
				Name = "Vintage Desk",
				Description = "Carve out a personal workspace with this storage desk. The simple design and classic mid-century modern details make this desk perfect for modern decor themes or casual open office settings, and the rectangular desktop provides space for a laptop and peripherals.",
				Price = 305,
				QtyInStock = 179
			});

			mockProductRepository.Setup(x => x.GetProduct(Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"))).ReturnsAsync(new Product
			{
				Id = Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
				Name = "Desk Techni",
				Description = "The Techni Mobili Complete Workstation Desk is everything you need in a computer desk and stay organized.",
				Price = 295,
				QtyInStock = 150
			});
		}

		[Test]
		public async Task GetOrder_ValidId_OrderDtoReturned()
		{
			//Arrange
			var orderId = Guid.Parse("98c981ce-8a7a-4aa5-ad78-204564ad0c5d");

			var sut = new OrderService(
				mockOrderRepository.Object,
				mockOrderStateRepository.Object,
				mockProductRepository.Object,
				mockMailService.Object,
				mockWarehouseService.Object
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
			var inDto = new buyyu.Models.OrderDto
			{
				ClientId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OrderDate = DateTime.MinValue,
				OrderId = Guid.Empty,
				OrderStateId = Guid.Empty,
				Orderlines = new List<OrderlineDto>
				{
					new OrderlineDto
					{
						OrderlineId = Guid.Empty,
						Price = 0m,
						ProductId = Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
						Qty = 10
					},
					new OrderlineDto
					{
						OrderlineId = Guid.Empty,
						Price = 0m,
						ProductId = Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"),
						Qty = 20
					}
				},
				PaidAmount = 0m,
				State = null,
				TotalAmount = 0m
			};

			var outDto = new buyyu.Models.OrderDto
			{
				ClientId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OrderDate = new DateTime(2001, 1, 1, 0, 0, 0),
				OrderId = Guid.Parse("5967544b-31b2-48ce-a746-2b6db6ff187a"),
				OrderStateId = NewOrderState.Id,
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
				State = NewOrderState.ShortCode,
				TotalAmount = 8210.00m
			};

			mockOrderRepository.Setup(x => x.GetOrderDto(It.IsAny<Guid>())).ReturnsAsync(outDto);

			var sut = new OrderService(
				mockOrderRepository.Object,
				mockOrderStateRepository.Object,
				mockProductRepository.Object,
				mockMailService.Object,
				mockWarehouseService.Object
				);

			//Act
			var returnedDto = await sut.CreateOrder(inDto);

			//Assert
			mockOrderRepository.Verify(x =>
				x.Save(It.Is<Order>(x =>
					x.ClientId == Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
					&& x.Id == Guid.Empty
					&& x.OrderStateId == NewOrderState.Id
					&& x.PaidAmount == 0
					&& x.Payments == null
					&& x.TotalAmount == 8210.00m
					&& x.Lines.Count == 2
					&& x.Lines[0].ProductId == Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e")
					&& x.Lines[0].Price == 295.00m
					&& x.Lines[0].Qty == 10
					&& x.Lines[1].ProductId == Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f")
					&& x.Lines[1].Price == 263.00m
					&& x.Lines[1].Qty == 20
				))
			);

			Assert.That(returnedDto, Is.EqualTo(outDto));
		}

		[Test]
		public async Task CreateOrder_OrderWithTwoProducts_OrderUpdated()
		{
			//Arrange
			var inDto = new buyyu.Models.OrderDto
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

			var outDto = new buyyu.Models.OrderDto
			{
				ClientId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OrderDate = new DateTime(2001, 1, 1, 0, 0, 0),
				OrderId = Guid.Parse("5967544b-31b2-48ce-a746-2b6db6ff187a"),
				OrderStateId = NewOrderState.Id,
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
				State = NewOrderState.ShortCode,
				TotalAmount = 8210.00m
			};

			var newOrder = new Order
			{
				ClientId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				Id = inDto.OrderId,
				Lines = new List<Orderline>
				{
					new Orderline
					{
						Id = Guid.Parse("fc0e1862-21c3-4cd0-ae9c-e02986b8f283"),
						Order = null,
						OrderId = Guid.Empty,
						Price = 295.00m,
						Product = null,
						ProductId = Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
						Qty = 10
					},
					new Orderline
					{
						Id = Guid.Parse("3510659d-d3c0-43bf-aca5-d2bbede87685"),
						Order = null,
						OrderId = Guid.Empty,
						Price = 263.00m,
						Product = null,
						ProductId = Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"),
						Qty = 20
					}
				},
				OrderDate = new DateTime(2021, 2, 19, 17, 26, 57),
				OrderStateId = Guid.Parse("bd8be3d2-8028-45e2-a211-bf737a2508c1"),
				PaidAmount = 0m,
				Payments = new List<Payment>(),
				State = NewOrderState,
				TotalAmount = 8210.00m
			};


			mockOrderRepository.Setup(x => x.GetOrderDto(It.IsAny<Guid>())).ReturnsAsync(outDto);
			mockOrderRepository.Setup(x => x.GetOrder(newOrder.Id)).ReturnsAsync(newOrder);

			var sut = new OrderService(
				mockOrderRepository.Object,
				mockOrderStateRepository.Object,
				mockProductRepository.Object,
				mockMailService.Object,
				mockWarehouseService.Object
				);

			//Act
			var returnedDto = await sut.UpdateOrder(inDto.OrderId, inDto);

			//Assert
			mockOrderRepository.Verify(x =>
				x.Save(It.Is<Order>(x =>
					x.ClientId == Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
					&& x.Id == Guid.Parse("3ed7fa03-fe11-441b-b59d-efab4928a6b9")
					&& x.OrderStateId == NewOrderState.Id
					&& x.PaidAmount == 0
					&& x.Payments.Count == 0
					&& x.TotalAmount == 23040m
					&& x.Lines.Count == 3
					&& x.Lines[0].ProductId == Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e")
					&& x.Lines[0].Price == 295.00m
					&& x.Lines[0].Qty == 10
					&& x.Lines[1].ProductId == Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f")
					&& x.Lines[1].Price == 263.00m
					&& x.Lines[1].Qty == 30
					&& x.Lines[2].ProductId == Guid.Parse("bcbc1851-6317-4022-be62-53d29c04bcda")
					&& x.Lines[2].Price == 305.00m
					&& x.Lines[2].Qty == 40
				))
			);

			Assert.That(returnedDto, Is.EqualTo(outDto));
		}
	}
}