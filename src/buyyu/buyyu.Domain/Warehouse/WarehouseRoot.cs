using buyyu.Domain.Shared;

namespace buyyu.Domain.Warehouse
{
	public class WarehouseRoot
	{
		public ProductId ProductId { get; private set; }
		public Quantity QtyInStock { get; private set; }

		public static WarehouseRoot StartNewProductStock(ProductId productId, Quantity quantity)
		{
			return new WarehouseRoot
			{
				ProductId = productId,
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
	}
}