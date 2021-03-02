using buyyu.DDD;
using System;

namespace buyyu.Domain.Order
{
	public class ProductId : Value<ProductId>
	{
		public Guid Value { get; private set; }

		//Satisfy EF Core
		private ProductId() { }

		private ProductId(Guid productId)
		{
			if (productId == null || productId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(productId), "ProductId cannot be empty");
			}

			Value = productId;
		}

		public static ProductId FromGuid(Guid productId) => new ProductId(productId);
		public static ProductId FromString(string productId) => new ProductId(Guid.Parse(productId));

		public static implicit operator Guid(ProductId productId) => productId.Value;
	}
}