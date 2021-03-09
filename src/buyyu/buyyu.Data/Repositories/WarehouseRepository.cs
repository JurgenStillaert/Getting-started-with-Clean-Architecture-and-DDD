using buyyu.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories
{
	public class WarehouseRepository : IWarehouseRepository
	{
		private readonly BuyyuDbContext _context;

		public WarehouseRepository(BuyyuDbContext context)
		{
			_context = context;
		}

		public async Task<bool> CheckProductStock(Guid productId, int amount)
		{
			var productStock = await _context.Warehouses.SingleOrDefaultAsync(x => x.Id == productId);
			if (productStock == null)
			{
				return false;
			}
			else
			{
				return productStock.QtyInStock.Value >= amount;
			}
		}
	}
}