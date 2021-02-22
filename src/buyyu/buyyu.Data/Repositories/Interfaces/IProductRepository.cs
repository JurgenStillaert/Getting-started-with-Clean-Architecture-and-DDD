using buyyu.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Interfaces
{
	public interface IProductRepository
	{
		Task<List<ProductDto>> GetAllProducts();
		Task<Product> GetProduct(Guid productId);
		Task<Product> Save(Product product);
	}
}