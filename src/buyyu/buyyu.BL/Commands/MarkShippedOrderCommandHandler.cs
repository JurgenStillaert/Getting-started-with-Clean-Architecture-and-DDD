using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using buyyu.Models.Commands;
using MediatR;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public sealed class MarkShippedOrderCommandHandler : UpdateCommandHandler<MarkShippedOrderCommand, OrderRoot, OrderId>
	{
		public MarkShippedOrderCommandHandler(
			IRepository<OrderRoot, OrderId> repo,
			IMediator mediator)
			: base(repo, mediator)
		{
		}

		protected override void PreHandle(MarkShippedOrderCommand command)
		{
			AggregateId = OrderId.FromGuid(command.OrderId);
		}

		protected async override Task Apply(MarkShippedOrderCommand command)
		{
			AggregateRoot.MarkShipped();
		}
	}
}