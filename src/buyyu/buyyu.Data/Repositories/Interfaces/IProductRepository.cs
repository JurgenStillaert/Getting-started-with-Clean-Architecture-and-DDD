using buyyu.Domain.Product;
using buyyu.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories.Interfaces
{
	public interface IProductRepository
	{
		Task<List<ProductDto>> GetAllProducts();
		Task<ProductRoot> GetProduct(Guid productId);
		Task<ProductRoot> Save(ProductRoot product);
	}
}