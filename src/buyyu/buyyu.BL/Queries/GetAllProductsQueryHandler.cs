using buyyu.Data.Repositories.Interfaces;
using buyyu.Models.Dtos;
using buyyu.Models.Queries;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace buyyu.BL.Queries
{
	public sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
	{
		private readonly IProductRepository _productRepository;

		public GetAllProductsQueryHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
		{
			return await _productRepository.GetAllProducts();
		}
	}
}