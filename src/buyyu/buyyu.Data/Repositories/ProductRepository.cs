using buyyu.Data.Repositories.Interfaces;
using buyyu.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buyyu.Data.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly BuyyuDbContext _context;

		public ProductRepository(BuyyuDbContext context)
		{
			_context = context;
		}

		public async Task<List<ProductDto>> GetAllProducts()
		{
			return await _context.Products.Select(prd =>
				new ProductDto
				{
					ProductId = prd.Id,
					Name = prd.Name,
					Description = prd.Description,
					Available = prd.QtyInStock,
					Price = prd.Price
				}
			).ToListAsync();
		}

		public async Task<Product> Save(Product product)
		{
			if (product.Id == null || product.Id == Guid.Empty)
			{
				await _context.AddAsync(product); 
			}
			await _context.SaveChangesAsync();

			return product;
		}

		public async Task<Product> GetProduct(Guid productId)
		{
			return await _context.Products.SingleAsync(prd => prd.Id == productId);
		}
	}
}