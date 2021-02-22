using buyyu.Data;
using System.Threading.Tasks;

namespace buyyu.BL.Interfaces
{
	public interface IMailService
	{
		Task SendPaymentConfirmationMail(Order order);
		Task SendOrderConfirmationMail(Order order);
		Task SendOrderShippedMail(Order order);
	}
}