using buyyu.Data.Repositories.Interfaces;
using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Shared;
using buyyu.Models.Commands;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public sealed class UpdateOrderCommandHandler : UpdateCommandHandler<UpdateOrderCommand, OrderRoot, OrderId>
	{
		private readonly IProductRepository _productRepository;

		public UpdateOrderCommandHandler(
			IRepository<OrderRoot, OrderId> repo,
			IProductRepository productRepository,
			IMediator mediator)
			: base(repo, mediator)
		{
			_productRepository = productRepository;
		}

		protected override void PreHandle(UpdateOrderCommand command)
		{
			AggregateId = OrderId.FromGuid(command.OrderId);
		}

		protected async override Task Apply(UpdateOrderCommand command)
		{
			//Remove orderlines
			var toRemoveOrderlineProducts = AggregateRoot.Lines.Select(ol => ol.ProductId.Value).Except(command.OrderLines.Select(ol => ol.ProductId));
			foreach (var toRemoveOrderlineProduct in toRemoveOrderlineProducts)
			{
				AggregateRoot.RemoveOrderline(ProductId.FromGuid(toRemoveOrderlineProduct));
			}

			//Update orderlines
			var toUpdateOrderlineProducts = command.OrderLines.Select(ol => ol.ProductId).Intersect(AggregateRoot.Lines.Select(ol => ol.ProductId.Value));
			foreach (var toUpdateOrderlineProduct in toUpdateOrderlineProducts)
			{
				var dtoOrderline = command.OrderLines.First(ol => ol.ProductId == toUpdateOrderlineProduct);
				var product = await _productRepository.GetProduct(toUpdateOrderlineProduct);
				AggregateRoot.UpdateOrderline(ProductId.FromGuid(product.Id), product.Price, Quantity.FromInt(dtoOrderline.Quantity));
			}

			//Add orderlines
			var toAddOrderlineProducts = command.OrderLines.Select(ol => ol.ProductId).Except(AggregateRoot.Lines.Select(ol => ol.ProductId.Value));
			foreach (var toAddOrderlineProduct in toAddOrderlineProducts)
			{
				var dtoOrderline = command.OrderLines.First(ol => ol.ProductId == toAddOrderlineProduct);
				var product = await _productRepository.GetProduct(toAddOrderlineProduct);
				AggregateRoot.AddOrderline(ProductId.FromGuid(product.Id), product.Price, Quantity.FromInt(dtoOrderline.Quantity));
			}
		}
	}
}