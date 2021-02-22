using buyyu.Models;
using System;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Interfaces
{
	public interface IOrderRepository
	{
		Task<OrderDto> GetOrderDto(Guid orderId);
		Task<Order> GetOrder(Guid orderId);
		Task Save(Order newOrder);
	}
}