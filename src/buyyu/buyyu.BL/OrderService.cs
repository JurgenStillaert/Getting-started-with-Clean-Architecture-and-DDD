using buyyu.BL.Interfaces;
using buyyu.Data.Repositories.Interfaces;
using buyyu.Domain.Order;
using buyyu.Domain.Shared;
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
		private readonly IOrderRepository _orderRepository;


		public OrderService(
			IOrderRepository orderRepository,
			IProductRepository productRepository,
			IMailService mailService,
			IWarehouseService warehouseService)
		{
			_orderRepository = orderRepository;
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
			var newOrder = OrderRoot.Create(OrderId.FromGuid(orderDto.OrderId), ClientId.FromGuid(orderDto.ClientId));
			foreach (var orderline in orderDto.Orderlines)
			{
				var product = await _productRepository.GetProduct(orderline.ProductId);
				newOrder.AddOrderline(ProductId.FromGuid(product.Id), Money.FromDecimalAndCurrency(product.Price, "EUR"), Quantity.FromInt(orderline.Qty));
			}

			await _orderRepository.Save(newOrder);

			return await GetOrder(newOrder.Id);
		}

		public async Task<OrderDto> UpdateOrder(Guid id, OrderDto orderDto)
		{
			var order = await _orderRepository.GetOrder(id);

			//Remove orderlines
			var toRemoveOrderlineProducts = order.Lines.Select(ol => ol.ProductId.Value).Except(orderDto.Orderlines.Select(ol => ol.ProductId));
			foreach (var toRemoveOrderlineProduct in toRemoveOrderlineProducts)
			{
				order.RemoveOrderline(ProductId.FromGuid(toRemoveOrderlineProduct));
			}

			//Update orderlines
			var toUpdateOrderlineProducts = orderDto.Orderlines.Select(ol => ol.ProductId).Intersect(order.Lines.Select(ol => ol.ProductId.Value));
			foreach (var toUpdateOrderlineProduct in toUpdateOrderlineProducts)
			{
				var dtoOrderline = orderDto.Orderlines.First(ol => ol.ProductId == toUpdateOrderlineProduct);
				var product = await _productRepository.GetProduct(toUpdateOrderlineProduct);
				order.UpdateOrderline(ProductId.FromGuid(product.Id), Money.FromDecimalAndCurrency(product.Price, "EUR"), Quantity.FromInt(dtoOrderline.Qty));
			}

			//Add orderlines
			var toAddOrderlineProducts = orderDto.Orderlines.Select(ol => ol.ProductId).Except(order.Lines.Select(ol => ol.ProductId.Value));
			foreach (var toAddOrderlineProduct in toAddOrderlineProducts)
			{
				var dtoOrderline = orderDto.Orderlines.First(ol => ol.ProductId == toAddOrderlineProduct);
				var product = await _productRepository.GetProduct(toAddOrderlineProduct);
				order.AddOrderline(ProductId.FromGuid(product.Id), Money.FromDecimalAndCurrency(product.Price, "EUR"), Quantity.FromInt(dtoOrderline.Qty));
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

			order.Confirm();

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

			order.Ship();

			await _orderRepository.Save(order);

			//Send email
			await _mailService.SendOrderShippedMail(order);

			return await GetOrder(order.Id);
		}

		public async Task<OrderDto> PayOrder(Guid id, decimal amount)
		{
			var order = await _orderRepository.GetOrder(id);

			order.ReceivePayment(Money.FromDecimalAndCurrency(amount, "EUR"));

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