using buyyu.DDD;
using System;

namespace buyyu.Domain.Product
{
	public class ProductRoot : AggregateRoot<ProductId>
	{
		//For now, we use a simple contsructor for this reference class
		public ProductRoot(ProductId id, string name, string description, decimal price, int qtyInStock)
		{
			Id = id;
			Name = name;
			Description = description;
			Price = price;
			QtyInStock = qtyInStock;
		}

		public string Name { get; private set; }
		public string Description { get; private set; }
		public decimal Price { get; private set; }
		public int QtyInStock { get; private set; }

		public void AddStock(int addedItems)
		{
			QtyInStock += addedItems;
		}

		public void ReduceStock(int removedItems)
		{
			QtyInStock -= removedItems;
		}

		protected override void EnsureValidation()
		{
			throw new NotImplementedException();
		}
	}
}