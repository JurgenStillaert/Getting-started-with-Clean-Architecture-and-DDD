using buyyu.BL.Interfaces;
using buyyu.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace buyyu.web.Controllers
{
	[ApiController]
	[Route("/api/[Controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<List<ProductDto>> GetProducts()
		{
			return await _productService.GetAllProducts();
		}
	}
}