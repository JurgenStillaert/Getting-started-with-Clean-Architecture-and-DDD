using buyyu.Domain.Payment;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Interfaces
{
	public interface IPaymentRepository
	{
		Task AddSave(PaymentRoot payment);
	}
}