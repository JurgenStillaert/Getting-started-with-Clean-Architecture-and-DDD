using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Commands
{
	public class OrderRepository : IRepository<OrderRoot, OrderId>
	{
		private readonly BuyyuDbContext _context;

		public OrderRepository(BuyyuDbContext context)
		{
			_context = context;
		}

		public async Task Add(OrderRoot aggregateRoot)
		{
			_context.Add(aggregateRoot);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(OrderRoot aggregateRoot)
		{
			_context.Remove(aggregateRoot);
			await _context.SaveChangesAsync();
		}

		public async Task<OrderRoot> Load(OrderId aggregateId)
		{
			return await _context.Orders.Include(x => x.Lines).FirstAsync(x => x.Id == aggregateId);
		}

		public async Task Save(OrderRoot aggregateRoot)
		{
			await _context.SaveChangesAsync();
		}
	}
}