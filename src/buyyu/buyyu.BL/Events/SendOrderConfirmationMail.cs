using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using static buyyu.Models.Events.OrderEvents;

namespace buyyu.BL.Events
{
	public class SendOrderConfirmationMail : INotificationHandler<v1.OrderConfirmed>
	{
		private readonly ILogger<SendOrderConfirmationMail> _logger;

		public SendOrderConfirmationMail(ILogger<SendOrderConfirmationMail> logger)
		{
			_logger = logger;
		}

		public async Task Handle(v1.OrderConfirmed notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Sending order confirmation");
		}
	}
}