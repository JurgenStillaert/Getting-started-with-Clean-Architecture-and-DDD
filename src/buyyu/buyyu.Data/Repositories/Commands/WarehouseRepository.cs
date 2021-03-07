using buyyu.DDD;
using buyyu.Domain.Shared;
using buyyu.Domain.Warehouse;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Commands
{
	public class WarehouseRepository : IRepository<WarehouseRoot, ProductId>
	{
		private readonly BuyyuDbContext _context;

		public WarehouseRepository(BuyyuDbContext context)
		{
			_context = context;
		}

		public async Task Add(WarehouseRoot aggregateRoot)
		{
			_context.Add(aggregateRoot);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(WarehouseRoot aggregateRoot)
		{
			_context.Remove(aggregateRoot);
			await _context.SaveChangesAsync();
		}

		public async Task<WarehouseRoot> Load(ProductId aggregateId)
		{
			return await _context.Warehouses.FirstAsync(x => x.Id == aggregateId);
		}

		public async Task Save(WarehouseRoot aggregateRoot)
		{
			await _context.SaveChangesAsync();
		}
	}
}