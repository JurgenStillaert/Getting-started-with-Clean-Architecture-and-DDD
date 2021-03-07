using buyyu.DDD;
using buyyu.Domain.Shared;
using buyyu.Domain.Shipment;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Commands
{
	public class ShipmentRepository : IRepository<ShipmentRoot, OrderId>
	{
		private readonly BuyyuDbContext _context;

		public ShipmentRepository(BuyyuDbContext context)
		{
			_context = context;
		}

		public async Task Add(ShipmentRoot aggregateRoot)
		{
			_context.Add(aggregateRoot);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(ShipmentRoot aggregateRoot)
		{
			_context.Remove(aggregateRoot);
			await _context.SaveChangesAsync();
		}

		public async Task<ShipmentRoot> Load(OrderId aggregateId)
		{
			return await _context.Shipments.FirstAsync(x => x.Id == aggregateId);
		}

		public async Task Save(ShipmentRoot aggregateRoot)
		{
			await _context.SaveChangesAsync();
		}
	}
}