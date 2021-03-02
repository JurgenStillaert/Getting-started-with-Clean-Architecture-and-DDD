using buyyu.Data.Repositories.Interfaces;
using buyyu.Domain.Order.Ref;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories
{
	public class OrderStateRepository : IOrderStateRepository
	{
		private readonly BuyyuDbContext _context;

		public OrderStateRepository(BuyyuDbContext context)
		{
			_context = context;
		}

		public async Task<OrderState> GetOrderStateByCode(string stateCode)
		{
			return await _context.OrderStates.SingleAsync(os => os.ShortCode == stateCode);
		}
	}
}