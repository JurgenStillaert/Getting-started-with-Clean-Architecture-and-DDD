using buyyu.Models.Commands;
using buyyu.Models.Dtos;
using buyyu.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace buyyu.web.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly IMediator _mediator;

		public OrderController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[Route("/{id}")]
		public async Task<OrderDto> GetOrder(Guid id)
		{
			return await _mediator.Send(new GetOrderDtoQuery(id));
		}

		[HttpPost]
		public async Task<OrderDto> CreateOrder(OrderDto order)
		{
			order.OrderId = Guid.NewGuid();

			var orderlineCommands = order.Orderlines.Select(x => new CreateOrderCommand.OrderLineCommand(x.ProductId, x.Qty)).ToList();
			var command = new CreateOrderCommand(order.OrderId, order.ClientId, orderlineCommands);
			await _mediator.Send(command);

			return await GetOrder(order.OrderId);
		}

		[HttpPut]
		[Route("/{id}")]
		public async Task<OrderDto> UpdateOrder(Guid id, OrderDto order)
		{
			var orderlineCommands = order.Orderlines.Select(x => new UpdateOrderCommand.OrderLineCommand(x.ProductId, x.Qty)).ToList();
			var command = new UpdateOrderCommand(id, order.ClientId, orderlineCommands);
			await _mediator.Send(command);

			return await GetOrder(id);
		}

		[HttpPost]
		[Route("/{id}/confirm")]
		public async Task<OrderDto> ConfirmOrder(Guid id)
		{
			await _mediator.Send(new ConfirmOrderCommand(id));

			return await GetOrder(id);
		}

		[HttpPost]
		[Route("/{id}/ship")]
		public async Task<OrderDto> ShipOrder(Guid id)
		{
			await _mediator.Send(new ShipOrderCommand(id));

			return await GetOrder(id);
		}

		[HttpPost]
		[Route("/{id}/pay")]
		public async Task<OrderDto> PayOrder(Guid id, decimal amount)
		{
			await _mediator.Send(new PaymentReceivedCommand(id, amount));

			return await GetOrder(id);
		}
	}
}