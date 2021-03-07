using buyyu.Data.Repositories.Interfaces;
using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using buyyu.Models.Commands;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public class CreateOrderCommandHandler : CreateCommandHandler<CreateOrderCommand, OrderRoot, OrderId>
	{
		private readonly IProductRepository _productRepository;

		public CreateOrderCommandHandler(
			IProductRepository productRepository,
			IRepository<OrderRoot, OrderId> orderRepository)
				: base(orderRepository)
		{
			_productRepository = productRepository;
		}

		protected async override Task Apply(CreateOrderCommand command)
		{
			AggregateRoot = OrderRoot.Create(OrderId.FromGuid(command.OrderId), ClientId.FromGuid(command.ClientId));
			foreach (var orderline in command.OrderLines)
			{
				var product = await _productRepository.GetProduct(orderline.ProductId);
				AggregateRoot.AddOrderline(ProductId.FromGuid(product.Id), product.Price, Quantity.FromInt(orderline.Quantity));
			}
		}
	}
}