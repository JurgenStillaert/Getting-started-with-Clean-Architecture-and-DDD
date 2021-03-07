using System;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Interfaces
{
	public interface IWarehouseRepository
	{
		Task<bool> CheckProductStock(Guid productId, int amount);
	}
}