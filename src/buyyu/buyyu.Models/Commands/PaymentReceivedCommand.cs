using MediatR;
using System;

namespace buyyu.Models.Commands
{
	public sealed class PaymentReceivedCommand : IRequest
	{
		public PaymentReceivedCommand(Guid orderId, decimal amount)
		{
			OrderId = orderId;
			Amount = amount;
		}

		public Guid OrderId { get; }
		public decimal Amount { get; }
	}
}