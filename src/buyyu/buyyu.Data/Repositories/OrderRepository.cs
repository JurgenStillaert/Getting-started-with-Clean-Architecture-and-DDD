using buyyu.Data.Repositories.Interfaces;
using buyyu.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly BuyyuDbContext _context;

		public OrderRepository(BuyyuDbContext context)
		{
			_context = context;
		}

		public async Task<OrderDto> GetOrderDto(Guid orderId)
		{
			return await _context.Orders.AsNoTracking() //AsNoTracking explicit, see https://github.com/dotnet/EntityFramework.Docs/issues/2205
				.Include(order => order.Lines)
				.Where(order => order.Id == orderId)
				.Select(order => new OrderDto
				{
					OrderId = order.Id,
					OrderDate = order.OrderDate,
					ClientId = order.ClientId,
					PaidAmount = order.PaidAmount.Amount,
					TotalAmount = order.TotalAmount.Amount,
					State = order.State.ToString(),
					Orderlines = order.Lines.Select(orderline => new OrderDto.OrderlineDto
					{
						OrderlineId = orderline.Id,
						Price = orderline.Price.Amount,
						ProductId = orderline.ProductId,
						Qty = orderline.Qty
					}).ToList()
				}).SingleAsync();
		}
	}
}