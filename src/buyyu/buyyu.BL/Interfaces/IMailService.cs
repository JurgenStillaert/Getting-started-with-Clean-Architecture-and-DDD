using buyyu.Domain.Order;
using System.Threading.Tasks;

namespace buyyu.BL.Interfaces
{
	public interface IMailService
	{
		Task SendPaymentConfirmationMail(OrderRoot order);
		Task SendOrderConfirmationMail(OrderRoot order);
		Task SendOrderShippedMail(OrderRoot order);
	}
}