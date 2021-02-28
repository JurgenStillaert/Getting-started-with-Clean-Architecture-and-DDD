using System;

namespace buyyu.Data
{
	public class Product
	{
		//For now, we use a simple contsructor for this reference class
		public Product(Guid id, string name, string description, decimal price, int qtyInStock)
		{
			Id = id;
			Name = name;
			Description = description;
			Price = price;
			QtyInStock = qtyInStock;
		}

		public Guid Id { get; private set; }
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
	}
}