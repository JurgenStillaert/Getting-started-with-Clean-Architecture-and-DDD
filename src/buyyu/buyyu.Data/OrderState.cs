using System;

namespace buyyu.Data
{
	public class OrderState
	{
		//For now, we use a simple contsructor for this reference class
		public OrderState(Guid id, string name, string shortCode)
		{
			Id = id;
			Name = name;
			ShortCode = shortCode;
		}

		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public string ShortCode { get; private set; }
	}
}