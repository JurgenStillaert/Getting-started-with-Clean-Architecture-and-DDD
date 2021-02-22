using buyyu.BL.Interfaces;
using buyyu.Data;
using buyyu.Data.Repositories.Interfaces;
using buyyu.Models;
using System;
using System.Collections.Generic;
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

			var newOrder = new Order
			{
				ClientId = orderDto.ClientId,
				OrderStateId = newOrderState.Id,
				OrderDate = DateTime.Now,
				PaidAmount = 0,
				Lines = new List<Orderline>()
			};
			await ProcessOrderLines(orderDto, newOrder);

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

			await ProcessOrderLines(orderDto, order);

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

			order.OrderStateId = confirmedOrderState.Id;
			order.OrderDate = DateTime.Now;

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

			order.OrderStateId = shippedOrderState.Id;

			await _orderRepository.Save(order);

			//Send email
			await _mailService.SendOrderShippedMail(order);

			return await GetOrder(order.Id);
		}

		public async Task<OrderDto> PayOrder(Guid id, decimal amount)
		{
			var order = await _orderRepository.GetOrder(id);

			order.PaidAmount += amount;

			order.Payments.Add(new Payment { PaidAmount = amount, PayDate = DateTime.Now });

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

		private async Task ProcessOrderLines(OrderDto orderDto, Order order)
		{
			foreach (var orderline in orderDto.Orderlines)
			{
				var product = await _productRepository.GetProduct(orderline.ProductId);

				if (product.QtyInStock < orderline.Qty)
				{
					throw new Exception("Not enough products in stock.");
				}

				if (orderline.OrderlineId != Guid.Empty)
				{
					var existingOrderLine = order.Lines.FirstOrDefault(x => x.Id == orderline.OrderlineId);
					existingOrderLine.ProductId = product.Id;
					existingOrderLine.Price = product.Price;
					existingOrderLine.Qty = orderline.Qty;
				}
				else
				{
					var existingOrderLine = order.Lines.FirstOrDefault(x => x.ProductId == orderline.ProductId);
					if (existingOrderLine != null)
					{
						existingOrderLine.ProductId = product.Id;
						existingOrderLine.Price = product.Price;
						existingOrderLine.Qty = orderline.Qty;
					}
					else
					{
						order.Lines.Add(new Orderline
						{
							ProductId = product.Id,
							Price = product.Price,
							Qty = orderline.Qty
						});
					}
				}
			}

			order.TotalAmount = order.Lines.Select(ol => ol.Price * ol.Qty).Sum();
		}
	}
}