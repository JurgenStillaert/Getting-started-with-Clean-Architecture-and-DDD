using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Interfaces
{
	public interface IOrderStateRepository
	{
		Task<OrderState> GetOrderStateByCode(string stateCode);
	}
}