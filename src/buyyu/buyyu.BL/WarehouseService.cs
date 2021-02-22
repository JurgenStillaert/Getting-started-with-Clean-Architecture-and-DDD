using buyyu.BL.Interfaces;
using buyyu.Data.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace buyyu.BL
{
	public class WarehouseService : IWarehouseService
	{
		private readonly IProductRepository _productRepository;

		public WarehouseService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task AddStock(Guid productId)
		{
			var product = await _productRepository.GetProduct(productId);

			product.QtyInStock += 200;

			await _productRepository.Save(product);
		}

		public async Task ReduceStock(Guid productId, int qty)
		{
			var product = await _productRepository.GetProduct(productId);

			product.QtyInStock -= qty;

			await _productRepository.Save(product);

			if (product.QtyInStock < 100)
			{
				await AddStock(productId);
			}
		}
	}
}