using buyyu.BL.Interfaces;
using buyyu.Domain.Order;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace buyyu.BL
{
	public class MailService : IMailService
	{
		private readonly ILogger<MailService> _logger;

		public MailService(ILogger<MailService> logger)
		{
			_logger = logger;
		}

		public async Task SendPaymentConfirmationMail(OrderRoot order)
		{
			_logger.LogInformation("Payment order confirmation");

			return;
		}

		public async Task SendOrderConfirmationMail(OrderRoot order)
		{
			_logger.LogInformation("Sending order confirmation");

			return;
		}

		public async Task SendOrderShippedMail(OrderRoot order)
		{
			_logger.LogInformation("Shipping order confirmation");

			return;
		}
	}
}