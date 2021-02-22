using System;
using System.Threading.Tasks;

namespace buyyu.BL.Interfaces
{
	public interface IWarehouseService
	{
		Task AddStock(Guid productId);
		Task ReduceStock(Guid productId, int qty);
	}
}