using buyyu.BL.Interfaces;
using buyyu.Data.Repositories.Interfaces;
using buyyu.Domain.Payment;
using buyyu.Domain.Shared;
using buyyu.Models;
using System;
using System.Threading.Tasks;

namespace buyyu.BL
{
	public class PaymentService : IPaymentService
	{
		private readonly IPaymentRepository _paymentRepository;
		private readonly IOrderService _orderService;
		private readonly IOrderRepository _orderRepository;

		public PaymentService(
			IPaymentRepository paymentRepository,
			IOrderRepository orderRepository,
			IOrderService orderService)
		{
			_paymentRepository = paymentRepository;
			_orderRepository = orderRepository;
			_orderService = orderService;
		}

		public async Task<OrderDto> PayOrder(Guid orderId, decimal amount)
		{
			var order = await _orderRepository.GetOrderDto(orderId);

			if (order.State != "CNF")
			{
				throw new InvalidOperationException("Cannot pay not confirmed order");
			}

			var payment = PaymentRoot.Create(PaymentId.CreateNew(), OrderId.FromGuid(orderId), Money.FromDecimalAndCurrency(amount, "EUR"));

			await _paymentRepository.AddSave(payment);

			//Unwanted coupling
			var orderDto = await _orderService.PayOrder(order.OrderId, amount);

			return orderDto;
		}
	}
}