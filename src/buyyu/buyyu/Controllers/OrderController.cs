using buyyu.BL.Interfaces;
using buyyu.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace buyyu.web.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly IPaymentService _paymentService;
		private readonly IWarehouseService _warehouseService;
		private readonly IOrderService _orderService;

		public OrderController(
			IOrderService orderService, 
			IWarehouseService warehouseService,
			IPaymentService paymentService)
		{
			_orderService = orderService;
			_warehouseService = warehouseService;
			_paymentService = paymentService;
		}

		[HttpGet]
		[Route("/{id}")]
		public async Task<OrderDto> GetOrder(Guid id)
		{
			return await _orderService.GetOrder(id);
		}

		[HttpPost]
		public async Task<OrderDto> CreateOrder(OrderDto order)
		{
			order.OrderId = Guid.NewGuid();
			return await _orderService.CreateOrder(order);
		}

		[HttpPut]
		[Route("/{id}")]
		public async Task<OrderDto> UpdateOrder(Guid id, OrderDto order)
		{
			return await _orderService.UpdateOrder(id, order);
		}

		[HttpPost]
		[Route("/{id}/confirm")]
		public async Task<OrderDto> ConfirmOrder(Guid id)
		{
			return await _orderService.ConfirmOrder(id);
		}

		[HttpPost]
		[Route("/{id}/ship")]
		public async Task<OrderDto> ShipOrder(Guid id)
		{
			return await _warehouseService.ShipOrder(id);
		}

		[HttpPost]
		[Route("/{id}/pay")]
		public async Task<OrderDto> PayOrder(Guid id, decimal amount)
		{
			return await _paymentService.PayOrder(id, amount);
		}
	}
}