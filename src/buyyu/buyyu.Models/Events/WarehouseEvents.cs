using buyyu.DDD;
using System;

namespace buyyu.Models.Events
{
	public static class WarehouseEvents
	{
		public static class v1
		{
			public sealed class NewProductStockStarted : IDomainEvent
			{
				public NewProductStockStarted(Guid productId, int quantity)
				{
					ProductId = productId;
					Quantity = quantity;
				}

				public Guid ProductId { get; }
				public int Quantity { get; }
			}

			public sealed class StockAdded : IDomainEvent
			{
				public StockAdded(Guid productId, int addedItems)
				{
					ProductId = productId;
					AddedItems = addedItems;
				}

				public Guid ProductId { get; }
				public int AddedItems { get; }
			}

			public sealed class StockReduced : IDomainEvent
			{
				public StockReduced(Guid productId, int removedItems)
				{
					ProductId = productId;
					RemovedItems = removedItems;
				}

				public Guid ProductId { get; }
				public int RemovedItems { get; }
			}
		}
	}
}