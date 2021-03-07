using System;

namespace buyyu.Models.Dtos
{
	public class ProductDto
	{
		public Guid ProductId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Available { get; set; }
	}
}
