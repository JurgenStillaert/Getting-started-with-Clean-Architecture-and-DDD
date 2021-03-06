using buyyu.Models;
using System;
using System.Threading.Tasks;

namespace buyyu.BL.Interfaces
{
	public interface IWarehouseService
	{
		Task<OrderDto> ShipOrder(Guid orderId);
		Task AddStock(Guid productId);
	}
}