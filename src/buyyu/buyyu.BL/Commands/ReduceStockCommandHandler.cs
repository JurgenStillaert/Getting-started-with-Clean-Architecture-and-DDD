using buyyu.DDD;
using buyyu.Domain.Shared;
using buyyu.Domain.Warehouse;
using buyyu.Models.Commands;
using System.Threading.Tasks;

namespace buyyu.BL.Commands
{
	public sealed class ReduceStockCommandHandler : UpdateCommandHandler<ReduceStockCommand, WarehouseRoot, ProductId>
	{
		public ReduceStockCommandHandler(IRepository<WarehouseRoot, ProductId> repo)
			: base(repo)
		{
		}

		protected override void PreHandle(ReduceStockCommand command)
		{
			AggregateId = ProductId.FromGuid(command.ProductId);
		}

		protected async override Task Apply(ReduceStockCommand command)
		{
			AggregateRoot.ReduceStock(Quantity.FromInt(command.Quantity));

			if (AggregateRoot.QtyInStock < 100)
			{
				AggregateRoot.AddStock(Quantity.FromInt(200));
			}
		}
	}
}