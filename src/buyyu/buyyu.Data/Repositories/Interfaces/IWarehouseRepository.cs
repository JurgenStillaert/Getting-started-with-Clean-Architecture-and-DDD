using buyyu.Domain.Warehouse;
using System;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Interfaces
{
	public interface IWarehouseRepository
	{
		Task<bool> CheckProductStock(Guid productId, int amount);
		Task<WarehouseRoot> GetWarehouseRootByProduct(Guid productId);
		Task Save(WarehouseRoot warehouseRoot);
	}
}