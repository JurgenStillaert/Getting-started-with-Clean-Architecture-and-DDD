using buyyu.Data.Repositories.Interfaces;
using buyyu.DDD;
using buyyu.Domain.Payment;
using buyyu.Domain.Shared;
using buyyu.Models.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public sealed class PaymentReceivedCommandHandler : CreateCommandHandler<PaymentReceivedCommand, PaymentRoot, PaymentId>
	{
		private readonly IMediator _mediator;
		private readonly IOrderRepository _orderRepository;

		public PaymentReceivedCommandHandler(
			IRepository<PaymentRoot, PaymentId> repo,
			IMediator mediator,
			IOrderRepository orderRepository)
			: base(repo, mediator)
		{
			_orderRepository = orderRepository;
			_mediator = mediator;
		}

		protected async override Task Apply(PaymentReceivedCommand command)
		{
			var order = await _orderRepository.GetOrderDto(command.OrderId);

			if (order.State == "NEW")
			{
				throw new InvalidOperationException("Cannot pay not confirmed order");
			}

			AggregateRoot = PaymentRoot.Create(
				PaymentId.CreateNew(),
				OrderId.FromGuid(command.OrderId),
				Money.FromDecimalAndCurrency(command.Amount, "EUR"));
		}
	}
}