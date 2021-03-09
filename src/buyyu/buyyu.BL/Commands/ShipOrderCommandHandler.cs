using buyyu.Data.Repositories.Interfaces;
using buyyu.DDD;
using buyyu.Domain.Shared;
using buyyu.Domain.Shipment;
using buyyu.Models.Commands;
using buyyu.Models.Dtos;
using MediatR;
using System;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public sealed class ShipOrderCommandHandler : CreateCommandHandler<ShipOrderCommand, ShipmentRoot, OrderId>
	{
		private readonly IMediator _mediator;
		private readonly IWarehouseRepository _warehouseRepository;
		private readonly IOrderRepository _orderRepository;

		public ShipOrderCommandHandler(
			IRepository<ShipmentRoot, OrderId> repo,
			IOrderRepository orderRepository,
			IWarehouseRepository warehouseRepository,
			IMediator mediator)
			: base(repo, mediator)
		{
			_orderRepository = orderRepository;
			_warehouseRepository = warehouseRepository;
			_mediator = mediator;
		}

		protected async override Task Apply(ShipOrderCommand command)
		{
			var order = await _orderRepository.GetOrderDto(command.OrderId);

			if (order.State != "CNF")
			{
				throw new InvalidOperationException("Cannot confirm not confirmed order");
			}

			foreach (var orderline in order.Orderlines)
			{
				await CheckProductStock(orderline);
			}

			AggregateRoot = ShipmentRoot.Ship(OrderId.FromGuid(command.OrderId));

			foreach (var orderline in order.Orderlines)
			{
				await _mediator.Send(new ReduceStockCommand(orderline.ProductId, orderline.Qty));
			}
		}

		private async Task CheckProductStock(OrderDto.OrderlineDto orderline)
		{
			var productInStock = await _warehouseRepository.CheckProductStock(orderline.ProductId, orderline.Qty);
			if (!productInStock)
			{
				throw new Exception("Not enough products in stock.");
			}
		}
	}
}