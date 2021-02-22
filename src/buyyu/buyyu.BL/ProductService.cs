using buyyu.BL.Interfaces;
using buyyu.Data.Repositories.Interfaces;
using buyyu.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace buyyu.BL
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<List<ProductDto>> GetAllProducts()
		{
			return await _productRepository.GetAllProducts();
		}
	}
}