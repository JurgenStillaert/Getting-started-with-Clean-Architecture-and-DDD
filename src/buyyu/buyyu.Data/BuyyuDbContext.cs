using Microsoft.EntityFrameworkCore;
using System;

namespace buyyu.Data
{
	public class BuyyuDbContext : DbContext
	{
		public BuyyuDbContext(DbContextOptions<BuyyuDbContext> options)
			: base(options)
		{
		}

		public DbSet<Order> Orders { get; set; }
		public DbSet<Orderline> OrderLines { get; set; }
		public DbSet<OrderState> OrderStates { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Payment> Payments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Products

			modelBuilder.Entity<Order>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			modelBuilder.Entity<Orderline>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			modelBuilder.Entity<Product>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			modelBuilder.Entity<Payment>().Property(x => x.Id).HasDefaultValueSql("NEWID()");


			modelBuilder.Entity<Product>()
				.HasData(
					new Product
					{
						Id = Guid.Parse("de679c55-4c13-4fe7-91b4-69cbce3223a2"),
						Name = "Office Chair Beta",
						Description = "Implement an ergonomic seating solution for your office with this maroon multipurpose chair. The included tilt tension knob lets you calibrate the tilt and recline resistance to your desired configuration, while the adjustable seat and armrests optimize your seating position for correct posture.",
						Price = 169,
						QtyInStock = 213
					},
					new Product
					{
						Id = Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"),
						Name = "Office Chair Manager",
						Description = "The Lockland Big & Tall bonded leather managers chair offers top quality comfort, multiple adjustment features.",
						Price = 263,
						QtyInStock = 75
					},
					new Product
					{
						Id = Guid.Parse("bcbc1851-6317-4022-be62-53d29c04bcda"),
						Name = "Vintage Desk",
						Description = "Carve out a personal workspace with this storage desk. The simple design and classic mid-century modern details make this desk perfect for modern decor themes or casual open office settings, and the rectangular desktop provides space for a laptop and peripherals.",
						Price = 305,
						QtyInStock = 179
					},
					new Product
					{
						Id = Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
						Name = "Desk Techni",
						Description = "The Techni Mobili Complete Workstation Desk is everything you need in a computer desk and stay organized.",
						Price = 295,
						QtyInStock = 150
					}
				);

			modelBuilder.Entity<OrderState>().HasData(
				new OrderState
				{
					Id = Guid.Parse("bd8be3d2-8028-45e2-a211-bf737a2508c1"),
					Name = "Initiated",
					ShortCode = "NEW"
				},
				new OrderState
				{
					Id = Guid.Parse("82d9ce01-9f25-48b1-8af3-93f52426676f"),
					Name = "Confirmed",
					ShortCode = "CNF"
				},
				new OrderState
				{
					Id = Guid.Parse("4b5549bb-b1b2-4964-9818-da984baab4ff"),
					Name = "Shipped",
					ShortCode = "SHP"
				}
			);
		}
	}
}