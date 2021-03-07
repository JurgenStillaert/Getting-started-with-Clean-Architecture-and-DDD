using buyyu.DDD;

namespace buyyu.Domain.Product
{
	public class Description : Value<Description>
	{
		public string Value { get; private set; }

		//Satisfy EF Core
		private Description() { }

		private Description(string description)
		{
			if (string.IsNullOrEmpty(description))
			{
				throw new System.ArgumentException($"'{nameof(description)}' cannot be null or empty", nameof(description));
			}

			if (description.Length < 20 || description.Length > 1000)
			{
				throw new System.ArgumentException($"Length of '{nameof(description)}' does not fall between 20 and 100", nameof(description));
			}

			Value = description;
		}

		public static Description FromString(string description) => new Description(description);

		public static implicit operator string(Description description) => description.Value;
	}
}