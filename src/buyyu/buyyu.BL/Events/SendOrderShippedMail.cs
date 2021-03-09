using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using static buyyu.Models.Events.OrderEvents;

namespace buyyu.BL.Events
{
	public class SendOrderShippedMail : INotificationHandler<v1.MarkedShipped>
	{
		private readonly ILogger<SendOrderShippedMail> _logger;

		public SendOrderShippedMail(ILogger<SendOrderShippedMail> logger)
		{
			_logger = logger;
		}

		public async Task Handle(v1.MarkedShipped notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Shipping order confirmation");
		}
	}
}