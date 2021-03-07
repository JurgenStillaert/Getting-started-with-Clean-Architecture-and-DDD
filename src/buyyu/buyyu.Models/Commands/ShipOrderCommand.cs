using MediatR;
using System;

namespace buyyu.Models.Commands
{
	public sealed class ShipOrderCommand : IRequest
	{
		public ShipOrderCommand(Guid orderId)
		{
			OrderId = orderId;
		}

		public Guid OrderId { get; }
	}
}