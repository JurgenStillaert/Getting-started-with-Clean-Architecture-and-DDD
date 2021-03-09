using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using buyyu.Models.Commands;
using MediatR;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public sealed class MarkPaidOrderCommandHandler : UpdateCommandHandler<MarkPaidOrderCommand, OrderRoot, OrderId>
	{

		public MarkPaidOrderCommandHandler(
			IRepository<OrderRoot, OrderId> repo,
			IMediator mediator)
			: base(repo, mediator)
		{
		}

		protected override void PreHandle(MarkPaidOrderCommand command)
		{
			AggregateId = OrderId.FromGuid(command.OrderId);
		}

		protected async override Task Apply(MarkPaidOrderCommand command)
		{
			AggregateRoot.MarkPaid(Money.FromDecimalAndCurrency(command.Amount, "EUR"));
		}
	}
}