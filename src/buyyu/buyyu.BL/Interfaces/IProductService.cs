using buyyu.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace buyyu.BL.Interfaces
{
	public interface IProductService
	{
		Task<List<ProductDto>> GetAllProducts();
	}
}