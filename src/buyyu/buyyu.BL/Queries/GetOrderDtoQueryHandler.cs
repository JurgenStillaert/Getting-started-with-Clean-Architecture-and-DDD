using buyyu.Data.Repositories.Interfaces;
using buyyu.Models.Dtos;
using buyyu.Models.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace buyyu.BL.Queries
{
	public class GetOrderDtoQueryHandler : IRequestHandler<GetOrderDtoQuery, OrderDto>
	{
		private readonly IOrderRepository _orderRepository;

		public GetOrderDtoQueryHandler(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task<OrderDto> Handle(GetOrderDtoQuery request, CancellationToken cancellationToken)
		{
			return await _orderRepository.GetOrderDto(request.OrderId);
		}
	}
}