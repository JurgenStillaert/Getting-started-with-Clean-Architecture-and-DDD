using buyyu.DDD;
using buyyu.Domain.Payment;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Commands
{
	public class PaymentRepository : IRepository<PaymentRoot, PaymentId>
	{
		private readonly BuyyuDbContext _context;

		public PaymentRepository(BuyyuDbContext context)
		{
			_context = context;
		}

		public async Task Add(PaymentRoot aggregateRoot)
		{
			_context.Add(aggregateRoot);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(PaymentRoot aggregateRoot)
		{
			_context.Remove(aggregateRoot);
			await _context.SaveChangesAsync();
		}

		public async Task<PaymentRoot> Load(PaymentId aggregateId)
		{
			return await _context.Payments.FirstAsync(x => x.Id == aggregateId);
		}

		public async Task Save(PaymentRoot aggregateRoot)
		{
			await _context.SaveChangesAsync();
		}
	}
}