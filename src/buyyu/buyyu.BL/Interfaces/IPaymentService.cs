using buyyu.Models;
using System;
using System.Threading.Tasks;

namespace buyyu.BL.Interfaces
{
	public interface IPaymentService
	{
		Task<OrderDto> PayOrder(Guid orderId, decimal amount);
	}
}