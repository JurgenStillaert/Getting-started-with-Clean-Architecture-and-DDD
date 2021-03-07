using MediatR;
using System;

namespace buyyu.Models.Commands
{
	public sealed class ConfirmOrderCommand : IRequest
	{
		public ConfirmOrderCommand(Guid orderId)
		{
			OrderId = orderId;
		}

		public Guid OrderId { get; }
	}
}