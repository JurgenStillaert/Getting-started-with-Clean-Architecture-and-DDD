using buyyu.Models.Dtos;
using buyyu.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace buyyu.web.Controllers
{
	[ApiController]
	[Route("/api/[Controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ProductsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<List<ProductDto>> GetProducts()
		{
			return await _mediator.Send(new GetAllProductsQuery());
		}
	}
}