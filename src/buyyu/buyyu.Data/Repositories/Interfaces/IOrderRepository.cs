using buyyu.Models.Dtos;
using System;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Interfaces
{
	public interface IOrderRepository
	{
		Task<OrderDto> GetOrderDto(Guid orderId);
	}
}