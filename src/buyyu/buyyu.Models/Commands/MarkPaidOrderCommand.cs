using MediatR;
using System;

namespace buyyu.Models.Commands
{
	public sealed class MarkPaidOrderCommand : IRequest
	{
		public MarkPaidOrderCommand(Guid orderId, decimal amount)
		{
			OrderId = orderId;
			Amount = amount;
		}

		public Guid OrderId { get; }
		public decimal Amount { get; }
	}
}