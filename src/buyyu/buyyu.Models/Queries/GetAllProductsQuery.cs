using buyyu.Models.Dtos;
using MediatR;
using System.Collections.Generic;

namespace buyyu.Models.Queries
{
	public sealed class GetAllProductsQuery: IRequest<List<ProductDto>>
	{
	}
}