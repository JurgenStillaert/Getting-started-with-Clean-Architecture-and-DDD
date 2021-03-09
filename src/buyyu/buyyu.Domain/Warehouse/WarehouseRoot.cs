using buyyu.DDD;
using buyyu.Domain.Shared;
using static buyyu.Models.Events.WarehouseEvents;

namespace buyyu.Domain.Warehouse
{
	public class WarehouseRoot : AggregateRoot<ProductId>
	{
		public Quantity QtyInStock { get; private set; }

		public static WarehouseRoot StartNewProductStock(ProductId productId, Quantity quantity)
		{
			var warehouse = new WarehouseRoot();

			warehouse.Apply(new v1.NewProductStockStarted(productId, quantity));

			return warehouse;
		}

		public void AddStock(Quantity addedItems)
		{
			Apply(new v1.StockAdded(Id, addedItems));
		}

		public void ReduceStock(Quantity removedItems)
		{
			Apply(new v1.StockAdded(Id, removedItems));
		}

		protected override void EnsureValidState()
		{
			if (QtyInStock == null)
			{
				throw new AggregateRootInvalidStateException();
			}
		}

		#region Handlers

		private void Handle(v1.NewProductStockStarted @event)
		{
			Id = ProductId.FromGuid(@event.ProductId);
			QtyInStock = Quantity.FromInt(@event.Quantity);
		}

		private void Handle(v1.StockAdded @event)
		{
			QtyInStock += Quantity.FromInt(@event.AddedItems);
		}

		private void Handle(v1.StockReduced @event)
		{
			QtyInStock -= Quantity.FromInt(@event.RemovedItems);
		}

		#endregion Handlers
	}
}