using buyyu.DDD;
using buyyu.Domain.Shared;

namespace buyyu.Domain.Product
{
	public class ProductRoot : AggregateRoot<ProductId>
	{
		//Satisfy EF
		private ProductRoot() { }

		//For now, we use a simple contsructor for this reference class
		public ProductRoot(
			ProductId id,
			ProductName name,
			Description description,
			Money price)
		{
			Id = id;
			Name = name;
			Description = description;
			Price = price;

			EnsureValidState();
		}

		public ProductName Name { get; private set; }
		public Description Description { get; private set; }
		public Money Price { get; private set; }

		protected override void EnsureValidState()
		{
			if (Id == null || Name == null || Description == null || Price == null)
			{
				throw new AggregateRootInvalidStateException();
			}
		}
	}
}