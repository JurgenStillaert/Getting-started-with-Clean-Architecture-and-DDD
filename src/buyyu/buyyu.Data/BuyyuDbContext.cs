using buyyu.Domain.Order;
using buyyu.Domain.Payment;
using buyyu.Domain.Product;
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

		public DbSet<OrderRoot> Orders { get; set; }
		public DbSet<Orderline> OrderLines { get; set; }
		public DbSet<Domain.Order.Ref.OrderState> OrderStates { get; set; }
		public DbSet<ProductRoot> Products { get; set; }
		public DbSet<PaymentRoot> Payments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductRoot>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			modelBuilder.Entity<PaymentRoot>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

			modelBuilder.Entity<ProductRoot>()
				.HasData(
					new ProductRoot
					(
						Domain.Product.ProductId.FromString("de679c55-4c13-4fe7-91b4-69cbce3223a2"),
						"Office Chair Beta",
						"Implement an ergonomic seating solution for your office with this maroon multipurpose chair. The included tilt tension knob lets you calibrate the tilt and recline resistance to your desired configuration, while the adjustable seat and armrests optimize your seating position for correct posture.",
						169,
						213
					),
					new ProductRoot
					(
						Domain.Product.ProductId.FromString("32f75bce-16a0-4070-9fac-4289678c191f"),
						"Office Chair Manager",
						"The Lockland Big & Tall bonded leather managers chair offers top quality comfort, multiple adjustment features.",
						263,
						75
					),
					new ProductRoot
					(
						Domain.Product.ProductId.FromString("bcbc1851-6317-4022-be62-53d29c04bcda"),
						"Vintage Desk",
						"Carve out a personal workspace with this storage desk. The simple design and classic mid-century modern details make this desk perfect for modern decor themes or casual open office settings, and the rectangular desktop provides space for a laptop and peripherals.",
						305,
						179
					),
					new ProductRoot
					(
						Domain.Product.ProductId.FromString("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
						"Desk Techni",
						"The Techni Mobili Complete Workstation Desk is everything you need in a computer desk and stay organized.",
						295,
						150
					)
				);

			modelBuilder.Entity<Domain.Order.Ref.OrderState>().HasData(
				new Domain.Order.Ref.OrderState
				(
					Guid.Parse("bd8be3d2-8028-45e2-a211-bf737a2508c1"),
					"Initiated",
					"NEW"
				),
				new Domain.Order.Ref.OrderState
				(
					Guid.Parse("82d9ce01-9f25-48b1-8af3-93f52426676f"),
					"Confirmed",
					"CNF"
				),
				new Domain.Order.Ref.OrderState
				(
					Guid.Parse("4b5549bb-b1b2-4964-9818-da984baab4ff"),
					"Shipped",
					"SHP"
				)
			);

			modelBuilder.Entity<OrderRoot>().OwnsOne(
				ol => ol.Id,
				id =>
				{
					id.Property(p => p.Value).HasColumnName("Id");
				});

			modelBuilder.Entity<OrderRoot>().OwnsOne(
				ol => ol.State,
				state =>
				{
					state.Property(p => p.Value).HasColumnName("State");
				});

			modelBuilder.Entity<OrderRoot>().OwnsOne(
				ol => ol.ClientId,
				clientId =>
				{
					clientId.Property(p => p.Value).HasColumnName("ClientId");
				});

			modelBuilder.Entity<OrderRoot>().OwnsOne(
				ol => ol.OrderDate,
				orderDate =>
				{
					orderDate.Property(p => p.Value).HasColumnName("OrderDate");
				});

			modelBuilder.Entity<OrderRoot>().OwnsOne(
				ol => ol.TotalAmount,
				totalAmount =>
				{
					totalAmount.Property(p => p.Amount).HasColumnName("TotalAmount");
					totalAmount.Property(p => p.Currency).HasColumnName("TotalAmount_Currency");
				});

			modelBuilder.Entity<OrderRoot>().OwnsOne(
				ol => ol.PaidAmount,
				paidAmount =>
				{
					paidAmount.Property(p => p.Amount).HasColumnName("PaidAmount");
					paidAmount.Property(p => p.Currency).HasColumnName("PaidAmount_Currency");
				});

			//modelBuilder.Entity<Orderline>().OwnsOne(
			//	ol => ol.Id,
			//	id =>
			//	{
			//		id.Property(p => p.Value).HasColumnName("Id");
			//	});

			modelBuilder.Entity<Orderline>().OwnsOne(
				ol => ol.OrderId,
				orderId =>
				{
					orderId.Property(p => p.Value).HasColumnName("ProductId");
				});

			modelBuilder.Entity<Orderline>().OwnsOne(
				ol => ol.Price,
				price =>
				{
					price.Property(p => p.Amount).HasColumnName("Price");
					price.Property(p => p.Currency).HasColumnName("Price_Currency");
				});

			modelBuilder.Entity<Orderline>().OwnsOne(
				ol => ol.ProductId,
				productId =>
				{
					productId.Property(p => p.Value).HasColumnName("ProductId");
				});

			//modelBuilder.Entity<ProductRoot>().OwnsOne(
			//	ol => ol.Id,
			//	id =>
			//	{
			//		id.Property(p => p.Value).HasColumnName("Id");
			//	});

			modelBuilder.Entity<PaymentRoot>().OwnsOne(
				ol => ol.Id,
				id =>
				{
					id.Property(p => p.Value).HasColumnName("Id");
				});
		}
	}
}