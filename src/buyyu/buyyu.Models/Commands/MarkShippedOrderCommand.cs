using MediatR;
using System;

namespace buyyu.Models.Commands
{
	public sealed class MarkShippedOrderCommand : IRequest
	{
		public MarkShippedOrderCommand(Guid orderId)
		{
			OrderId = orderId;
		}

		public Guid OrderId { get; }
	}
}