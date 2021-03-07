using buyyu.Models.Dtos;
using MediatR;
using System;

namespace buyyu.Models.Queries
{
	public class GetOrderDtoQuery : IRequest<OrderDto>
	{
		public GetOrderDtoQuery(Guid orderId)
		{
			OrderId = orderId;
		}

		public Guid OrderId { get; }
	}
}