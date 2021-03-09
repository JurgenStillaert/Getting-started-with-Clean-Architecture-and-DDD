using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using buyyu.Models.Commands;
using MediatR;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public sealed class ConfirmOrderCommandHandler : UpdateCommandHandler<ConfirmOrderCommand, OrderRoot, OrderId>
	{

		public ConfirmOrderCommandHandler(
			IRepository<OrderRoot, OrderId> repo,
			IMediator mediator)
			: base(repo, mediator)
		{
		}

		protected override void PreHandle(ConfirmOrderCommand command)
		{
			AggregateId = OrderId.FromGuid(command.OrderId);
		}

		protected async override Task Apply(ConfirmOrderCommand command)
		{
			AggregateRoot.Confirm();
		}
	}
}