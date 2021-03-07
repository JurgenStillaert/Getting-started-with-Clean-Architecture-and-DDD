using buyyu.DDD;
using buyyu.Domain.Shared;

namespace buyyu.Domain.Warehouse
{
	public class WarehouseRoot : AggregateRoot<ProductId>
	{
		public Quantity QtyInStock { get; private set; }

		public static WarehouseRoot StartNewProductStock(ProductId productId, Quantity quantity)
		{
			return new WarehouseRoot
			{
				Id = productId,
				QtyInStock = quantity
			};
		}

		public void AddStock(Quantity addedItems)
		{
			QtyInStock += addedItems;
		}

		public void ReduceStock(Quantity removedItems)
		{
			QtyInStock -= removedItems;
		}

		protected override void EnsureValidState()
		{
			if (QtyInStock == null)
			{
				throw new AggregateRootInvalidStateException();
			}
		}
	}
}