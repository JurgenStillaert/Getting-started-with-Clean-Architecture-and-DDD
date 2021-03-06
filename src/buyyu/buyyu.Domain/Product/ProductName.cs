using buyyu.DDD;

namespace buyyu.Domain.Product
{
	public class ProductName : Value<ProductName>
	{
		public string Value { get; private set; }

		//Satisfy EF Core
		private ProductName() { }

		private ProductName(string productName)
		{
			if (string.IsNullOrEmpty(productName))
			{
				throw new System.ArgumentException($"'{nameof(productName)}' cannot be null or empty", nameof(productName));
			}

			if (productName.Length < 3 || productName.Length > 100)
			{
				throw new System.ArgumentException($"Length of '{nameof(productName)}' does not fall between 3 and 100", nameof(productName));
			}

			Value = productName;
		}

		public static ProductName FromString(string productName) => new ProductName(productName);

		public static implicit operator string(ProductName productName) => productName.Value;
	}
}