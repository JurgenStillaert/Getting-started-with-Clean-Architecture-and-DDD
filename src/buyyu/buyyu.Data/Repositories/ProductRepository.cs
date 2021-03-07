using buyyu.Data.Repositories.Interfaces;
using buyyu.Domain.Product;
using buyyu.Models.Dtos;
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
			return await _context.Products.AsNoTracking()
				.Join(
					_context.Warehouses.AsNoTracking(),
					prod => prod.Id,
					wh => wh.Id,
					(prod, wh) => new ProductDto
					{
						ProductId = prod.Id,
						Name = prod.Name,
						Description = prod.Description,
						Price = prod.Price.Amount,
						Available = wh.QtyInStock
					}
			).ToListAsync();
		}

		public async Task<ProductRoot> Save(ProductRoot product)
		{
			if (product.Id == null || product.Id == Guid.Empty)
			{
				await _context.AddAsync(product); 
			}
			await _context.SaveChangesAsync();

			return product;
		}

		public async Task<ProductRoot> GetProduct(Guid productId)
		{
			return await _context.Products.SingleAsync(prd => prd.Id == productId);
		}
	}
}