using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using static buyyu.Models.Events.OrderEvents;

namespace buyyu.BL.Events
{
	public class SendPaymentConfirmationMail : INotificationHandler<v1.MarkedPaid>
	{
		private readonly ILogger<SendPaymentConfirmationMail> _logger;

		public SendPaymentConfirmationMail(ILogger<SendPaymentConfirmationMail> logger)
		{
			_logger = logger;
		}

		public async Task Handle(v1.MarkedPaid notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Payment order confirmation");
		}
	}
}