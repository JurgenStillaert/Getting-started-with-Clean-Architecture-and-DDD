using buyyu.Data.Repositories.Interfaces;
using buyyu.Models;
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

		public async Task<Order> GetOrder(Guid orderId)
		{
			return await _context
				.Orders
				.Include(order => order.Lines)
				.Include(order => order.Payments)
				.Include(order => order.State)
				.SingleAsync(x => x.Id == orderId);
		}

		public async Task<OrderDto> GetOrderDto(Guid orderId)
		{
			return await _context.Orders
				.Include(order => order.Lines)
				.Where(order => order.Id == orderId)
				.Select(order => new OrderDto
				{
					OrderId = order.Id,
					OrderDate = order.OrderDate,
					ClientId = order.ClientId,
					OrderStateId = order.OrderStateId,
					PaidAmount = order.PaidAmount,
					TotalAmount = order.TotalAmount,
					State = order.State.ShortCode,
					Orderlines = order.Lines.Select(orderline => new OrderDto.OrderlineDto
					{
						OrderlineId = orderline.Id,
						Price = orderline.Price,
						ProductId = orderline.ProductId,
						Qty = orderline.Qty
					}).ToList()
				}).SingleAsync();
		}

		public async Task Save(Order order)
		{
			if (order.Id == null || order.Id == Guid.Empty)
			{
				await _context.AddAsync(order);
			}
			await _context.SaveChangesAsync();
		}
	}
}