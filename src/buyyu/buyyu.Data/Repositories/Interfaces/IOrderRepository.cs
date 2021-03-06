using buyyu.Domain.Order;
using buyyu.Models;
using System;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Interfaces
{
	public interface IOrderRepository
	{
		Task<OrderDto> GetOrderDto(Guid orderId);
		Task<OrderRoot> GetOrder(Guid orderId);
		Task Save(OrderRoot newOrder);
		Task AddSave(OrderRoot newOrder);
	}
}