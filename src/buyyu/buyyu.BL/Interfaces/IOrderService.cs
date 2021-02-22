using buyyu.Models;
using System;
using System.Threading.Tasks;

namespace buyyu.BL.Interfaces
{
	public interface IOrderService
	{
		Task<OrderDto> GetOrder(Guid orderId);
		Task<OrderDto> CreateOrder(OrderDto order);
		Task<OrderDto> ConfirmOrder(Guid id);
		Task<OrderDto> ShipOrder(Guid id);
		Task<OrderDto> UpdateOrder(Guid id, OrderDto order);
		Task<OrderDto> PayOrder(Guid id, decimal amount);
	}
}