using buyyu.BL.Interfaces;
using buyyu.Data;
using buyyu.Data.Repositories.Interfaces;
using buyyu.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace buyyu.BL
{
	public class OrderService : IOrderService
	{
		private readonly IMailService _mailService;
		private readonly IWarehouseService _warehouseService;
		private readonly IProductRepository _productRepository;
		private readonly IOrderStateRepository _orderStateRepository;
		private readonly IOrderRepository _orderRepository;

		private const string NEWSTATECODE = "NEW";
		private const string CONFIRMEDSTATECODE = "CNF";
		private const string SHIPPEDSTATECODE = "SHP";

		public OrderService(
			IOrderRepository orderRepository,
			IOrderStateRepository orderStateRepository,
			IProductRepository productRepository,
			IMailService mailService,
			IWarehouseService warehouseService)
		{
			_orderRepository = orderRepository;
			_orderStateRepository = orderStateRepository;
			_productRepository = productRepository;
			_mailService = mailService;
			_warehouseService = warehouseService;
		}

		public async Task<OrderDto> GetOrder(Guid orderId)
		{
			return await _orderRepository.GetOrderDto(orderId);
		}

		public async Task<OrderDto> CreateOrder(OrderDto orderDto)
		{
			var newOrderState = await _orderStateRepository.GetOrderStateByCode(NEWSTATECODE);

			var newOrder = Order.Create(orderDto.ClientId, newOrderState.Id);
			foreach (var orderline in orderDto.Orderlines)
			{
				var product = await _productRepository.GetProduct(orderline.ProductId);
				newOrder.AddOrderline(product.Id, product.Price, orderline.Qty);
			}

			await _orderRepository.Save(newOrder);

			return await GetOrder(newOrder.Id);
		}

		public async Task<OrderDto> UpdateOrder(Guid id, OrderDto orderDto)
		{
			var order = await _orderRepository.GetOrder(id);

			if (order.State.ShortCode != NEWSTATECODE)
			{
				throw new Exception("Cannot change a confirmed order");
			}

			//Remove orderlines
			var toRemoveOrderlineProducts = order.Lines.Select(ol => ol.ProductId).Except(orderDto.Orderlines.Select(ol => ol.ProductId));
			foreach (var toRemoveOrderlineProduct in toRemoveOrderlineProducts)
			{
				order.RemoveOrderline(toRemoveOrderlineProduct);
			}

			//Update orderlines
			var toUpdateOrderlineProducts = orderDto.Orderlines.Select(ol => ol.ProductId).Intersect(order.Lines.Select(ol => ol.ProductId));
			foreach (var toUpdateOrderlineProduct in toUpdateOrderlineProducts)
			{
				var dtoOrderline = orderDto.Orderlines.First(ol => ol.ProductId == toUpdateOrderlineProduct);
				var product = await _productRepository.GetProduct(toUpdateOrderlineProduct);
				order.UpdateOrderline(product.Id, product.Price, dtoOrderline.Qty);
			}

			//Add orderlines
			var toAddOrderlineProducts = orderDto.Orderlines.Select(ol => ol.ProductId).Except(order.Lines.Select(ol => ol.ProductId));
			foreach (var toAddOrderlineProduct in toAddOrderlineProducts)
			{
				var dtoOrderline = orderDto.Orderlines.First(ol => ol.ProductId == toAddOrderlineProduct);
				var product = await _productRepository.GetProduct(toAddOrderlineProduct);
				order.AddOrderline(product.Id, product.Price, dtoOrderline.Qty);
			}

			await _orderRepository.Save(order);

			return await GetOrder(order.Id);
		}

		public async Task<OrderDto> ConfirmOrder(Guid id)
		{
			var order = await _orderRepository.GetOrder(id);

			//Check if the order has products
			if (order.Lines.DefaultIfEmpty().Sum(ol => ol.Qty) == 0)
			{
				throw new Exception("Order does not have any products and cannot be confirmed");
			}

			var confirmedOrderState = await _orderStateRepository.GetOrderStateByCode(CONFIRMEDSTATECODE);

			order.Confirm(confirmedOrderState.Id);

			await _orderRepository.Save(order);

			//Send email
			await _mailService.SendOrderConfirmationMail(order);

			return await GetOrder(order.Id);
		}

		public async Task<OrderDto> ShipOrder(Guid id)
		{
			var order = await _orderRepository.GetOrder(id);

			//Check if the there is enough stock
			foreach (var orderline in order.Lines)
			{
				await CheckProductStock(orderline);
			}

			//Everything ok, reduce items in stock
			foreach (var orderline in order.Lines)
			{
				await ReduceProductStock(orderline);
			}

			var shippedOrderState = await _orderStateRepository.GetOrderStateByCode(SHIPPEDSTATECODE);

			order.Ship(shippedOrderState.Id);

			await _orderRepository.Save(order);

			//Send email
			await _mailService.SendOrderShippedMail(order);

			return await GetOrder(order.Id);
		}

		public async Task<OrderDto> PayOrder(Guid id, decimal amount)
		{
			var order = await _orderRepository.GetOrder(id);

			order.ReceivePayment(amount);

			await _orderRepository.Save(order);

			//Send email
			await _mailService.SendPaymentConfirmationMail(order);

			return await GetOrder(order.Id);
		}

		private async Task ReduceProductStock(Orderline ol)
		{
			await _warehouseService.ReduceStock(ol.ProductId, ol.Qty);
		}

		private async Task CheckProductStock(Orderline orderline)
		{
			var product = await _productRepository.GetProduct(orderline.ProductId);
			if (product.QtyInStock < orderline.Qty)
			{
				throw new Exception("Not enough products in stock.");
			}
		}
	}
}