using MediatR;
using System;

namespace buyyu.Models.Commands
{
	public sealed class ReduceStockCommand : IRequest
	{
		public ReduceStockCommand(Guid productId, int quantity)
		{
			ProductId = productId;
			Quantity = quantity;
		}

		public Guid ProductId { get; }
		public int Quantity { get; }
	}
}