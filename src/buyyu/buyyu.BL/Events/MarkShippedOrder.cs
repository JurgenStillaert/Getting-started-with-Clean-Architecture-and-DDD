using buyyu.Models.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static buyyu.Models.Events.ShipmentEvents;

namespace buyyu.BL.Events
{
	public class MarkShippedOrder : INotificationHandler<v1.OrderShipped>
	{
		private readonly IMediator _mediator;

		public MarkShippedOrder(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task Handle(v1.OrderShipped notification, CancellationToken cancellationToken)
		{
			await _mediator.Send(new MarkShippedOrderCommand(notification.OrderId));
		}
	}
}