using MediatR;
using System;
using System.Collections.Generic;

namespace buyyu.Models.Commands
{
	public sealed class UpdateOrderCommand : IRequest
	{
		public UpdateOrderCommand(Guid orderId, Guid clientId, List<OrderLineCommand> orderLines)
		{
			OrderId = orderId;
			ClientId = clientId;
			OrderLines = orderLines;
		}

		public Guid OrderId { get; }
		public Guid ClientId { get; }
		public List<OrderLineCommand> OrderLines { get; }

		public sealed class OrderLineCommand
		{
			public OrderLineCommand(Guid productId, int quantity)
			{
				ProductId = productId;
				Quantity = quantity;
			}

			public Guid ProductId { get; }
			public int Quantity { get; }
		}
	}
}