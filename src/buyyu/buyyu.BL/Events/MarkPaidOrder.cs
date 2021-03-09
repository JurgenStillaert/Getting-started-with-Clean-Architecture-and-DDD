using buyyu.Models.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static buyyu.Models.Events.PaymentEvents;

namespace buyyu.BL.Events
{
	public class MarkPaidOrder : INotificationHandler<v1.PaymentCreated>
	{
		private readonly IMediator _mediator;

		public MarkPaidOrder(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task Handle(v1.PaymentCreated notification, CancellationToken cancellationToken)
		{
			await _mediator.Send(new MarkPaidOrderCommand(notification.OrderId, notification.PaidAmount));
		}
	}
}