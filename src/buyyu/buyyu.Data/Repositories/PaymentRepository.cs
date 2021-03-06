using buyyu.Data.Repositories.Interfaces;
using buyyu.Domain.Payment;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories
{
	public class PaymentRepository : IPaymentRepository
	{
		private readonly BuyyuDbContext _context;

		public PaymentRepository(BuyyuDbContext context)
		{
			_context = context;
		}

		public async Task AddSave(PaymentRoot payment)
		{
			await _context.AddAsync(payment);
			await _context.SaveChangesAsync();
		}
	}
}