using buyyu.BL.Interfaces;
using buyyu.Data;
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

		public async Task SendPaymentConfirmationMail(Order order)
		{
			_logger.LogInformation("Payment order confirmation");

			return;
		}

		public async Task SendOrderConfirmationMail(Order order)
		{
			_logger.LogInformation("Sending order confirmation");

			return;
		}

		public async Task SendOrderShippedMail(Order order)
		{
			_logger.LogInformation("Shipping order confirmation");

			return;
		}
	}
}