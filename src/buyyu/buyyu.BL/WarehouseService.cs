using buyyu.BL.Interfaces;
using buyyu.Data.Repositories.Interfaces;
using buyyu.Domain.Shared;
using buyyu.Models;
using System;
using System.Threading.Tasks;

namespace buyyu.BL
{
	public class WarehouseService : IWarehouseService
	{
		private readonly IOrderService _orderService;
		private readonly IOrderRepository _orderRepository;
		private readonly IWarehouseRepository _warehouseRepository;

		public WarehouseService(
			IWarehouseRepository warehouseRepository,
			IOrderRepository orderRepository,
			IOrderService orderService)
		{
			_warehouseRepository = warehouseRepository;
			_orderRepository = orderRepository;
			_orderService = orderService;
		}

		public async Task<OrderDto> ShipOrder(Guid orderId)
		{
			var order = await _orderRepository.GetOrderDto(orderId);

			if (order.State != "CNF")
			{
				throw new InvalidOperationException("Cannot confirm not confirmed order");
			}

			foreach (var orderline in order.Orderlines)
			{
				await CheckProductStock(orderline);
			}

			foreach (var orderline in order.Orderlines)
			{
				await ReduceStock(orderline.ProductId, orderline.Qty);
			}

			//Unwanted coupling
			var orderDto = await _orderService.ShipOrder(order.OrderId);

			return orderDto;
		}

		public async Task AddStock(Guid productId)
		{
			var warehouse = await _warehouseRepository.GetWarehouseRootByProduct(productId);

			warehouse.AddStock(Quantity.FromInt(200));

			await _warehouseRepository.Save(warehouse);
		}

		private async Task CheckProductStock(OrderDto.OrderlineDto orderline)
		{
			var productInStock = await _warehouseRepository.CheckProductStock(orderline.ProductId, orderline.Qty);
			if (!productInStock)
			{
				throw new Exception("Not enough products in stock.");
			}
		}

		private async Task ReduceStock(Guid productId, int qty)
		{
			var warehouse = await _warehouseRepository.GetWarehouseRootByProduct(productId);

			warehouse.ReduceStock(Quantity.FromInt(200));

			if (warehouse.QtyInStock < 100)
			{
				await AddStock(productId);
			}

			await _warehouseRepository.Save(warehouse);
		}
	}
}