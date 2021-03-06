using buyyu.Domain.Order;
using buyyu.Domain.Payment;
using buyyu.Domain.Product;
using buyyu.Domain.Shared;
using buyyu.Domain.Warehouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
		public DbSet<WarehouseRoot> Warehouses { get; set; }

		protected override void OnModelCreating(ModelBuilder mb)
		{
			ConfigureProductRoot(mb.Entity<ProductRoot>());
			ConfigureWarehouseRoot(mb.Entity<WarehouseRoot>());
			ConfigurePaymentRoot(mb.Entity<PaymentRoot>());
			ConfigureOrderRoot(mb.Entity<OrderRoot>());
			ConfigureOrderline(mb.Entity<Orderline>());
			ConfigureOrderState(mb.Entity<Domain.Order.Ref.OrderState>());
		}

		private static void ConfigureOrderState(EntityTypeBuilder<Domain.Order.Ref.OrderState> mb)
		{
			mb.HasData(
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
		}

		private static void ConfigureWarehouseRoot(EntityTypeBuilder<WarehouseRoot> mb)
		{
			mb.HasKey(x => x.ProductId);
			mb.Property(x => x.ProductId).HasConversion(x => x.Value, s => ProductId.FromGuid(s));

			mb.HasData(
					new { ProductId = ProductId.FromString("de679c55-4c13-4fe7-91b4-69cbce3223a2") },
					new { ProductId = ProductId.FromString("32f75bce-16a0-4070-9fac-4289678c191f") },
					new { ProductId = ProductId.FromString("bcbc1851-6317-4022-be62-53d29c04bcda") },
					new { ProductId = ProductId.FromString("5ca659b1-25b1-45c1-9755-3a3cd8591b9e") }
					);

			mb.OwnsOne(
				wh => wh.QtyInStock,
				qty =>
				{
					qty.Property(o => o.Value).HasColumnName("QtyInStock");
					qty.HasData(
						new { WarehouseRootProductId = ProductId.FromString("de679c55-4c13-4fe7-91b4-69cbce3223a2"), Value = 213 },
						new { WarehouseRootProductId = ProductId.FromString("32f75bce-16a0-4070-9fac-4289678c191f"), Value = 75 },
						new { WarehouseRootProductId = ProductId.FromString("bcbc1851-6317-4022-be62-53d29c04bcda"), Value = 179 },
						new { WarehouseRootProductId = ProductId.FromString("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"), Value = 150 }
					);
				});
		}

		private static void ConfigureOrderline(EntityTypeBuilder<Orderline> mb)
		{
			mb.HasKey(x => x.Id);
			mb.Property(x => x.Id).HasConversion(x => x.Value, s => OrderlineId.FromGuid(s));

			mb.OwnsOne(
				ol => ol.Qty,
				qty =>
				{
					qty.Property(p => p.Value).HasColumnName("Qty");
				});

			mb.OwnsOne(
				ol => ol.Price,
				price =>
				{
					price.Property(p => p.Amount).HasColumnName("Price");
					price.Property(p => p.Currency).HasColumnName("Price_Currency");
				});

			mb.OwnsOne(
				ol => ol.ProductId,
				productId =>
				{
					productId.Property(p => p.Value).HasColumnName("ProductId");
				});
		}

		private static void ConfigureOrderRoot(EntityTypeBuilder<OrderRoot> mb)
		{
			mb.HasKey(x => x.Id);
			mb.Property(x => x.Id).HasConversion(x => x.Value, s => OrderId.FromGuid(s));

			mb.OwnsOne(
				ol => ol.State,
				state =>
				{
					state.Property(p => p.Value).HasColumnName("State");
				});

			mb.OwnsOne(
				ol => ol.ClientId,
				clientId =>
				{
					clientId.Property(p => p.Value).HasColumnName("ClientId");
				});

			mb.OwnsOne(
				ol => ol.OrderDate,
				orderDate =>
				{
					orderDate.Property(p => p.Value).HasColumnName("OrderDate");
				});

			mb.OwnsOne(
				ol => ol.TotalAmount,
				totalAmount =>
				{
					totalAmount.Property(p => p.Amount).HasColumnName("TotalAmount");
					totalAmount.Property(p => p.Currency).HasColumnName("TotalAmount_Currency");
				});

			mb.OwnsOne(
				ol => ol.PaidAmount,
				paidAmount =>
				{
					paidAmount.Property(p => p.Amount).HasColumnName("PaidAmount");
					paidAmount.Property(p => p.Currency).HasColumnName("PaidAmount_Currency");
				});
		}

		private static void ConfigurePaymentRoot(EntityTypeBuilder<PaymentRoot> mb)
		{
			mb.Property(x => x.Id).HasDefaultValueSql("NEWID()");

			mb.HasKey(x => x.Id);
			mb.Property(x => x.Id).HasConversion(x => x.Value, s => PaymentId.FromGuid(s));

			mb.OwnsOne(
				pr => pr.OrderId,
				orderId =>
				{
					orderId.Property(o => o.Value).HasColumnName("OrderId");
				});

			mb.OwnsOne(
				pr => pr.PaidAmount,
				paidAmount =>
				{
					paidAmount.Property(p => p.Amount).HasColumnName("PaidAmount");
					paidAmount.Property(p => p.Currency).HasColumnName("PaidAmount_Currency");
				});
		}

		private static void ConfigureProductRoot(EntityTypeBuilder<ProductRoot> mb)
		{
			mb.HasKey(x => x.Id);
			mb.Property(x => x.Id).HasConversion(x => x.Value, s => ProductId.FromGuid(s));

			mb.HasData(
					new { Id = ProductId.FromString("de679c55-4c13-4fe7-91b4-69cbce3223a2") },
					new { Id = ProductId.FromString("32f75bce-16a0-4070-9fac-4289678c191f") },
					new { Id = ProductId.FromString("bcbc1851-6317-4022-be62-53d29c04bcda") },
					new { Id = ProductId.FromString("5ca659b1-25b1-45c1-9755-3a3cd8591b9e") }
				);

			mb.OwnsOne(
				pr => pr.Name,
				name =>
				{
					name.Property(x => x.Value).HasColumnName("Name");
					name.HasData(
						new { ProductRootId = ProductId.FromString("de679c55-4c13-4fe7-91b4-69cbce3223a2"), Value = "Office Chair Beta" },
						new { ProductRootId = ProductId.FromString("32f75bce-16a0-4070-9fac-4289678c191f"), Value = "Office Chair Manager" },
						new { ProductRootId = ProductId.FromString("bcbc1851-6317-4022-be62-53d29c04bcda"), Value = "Vintage Desk" },
						new { ProductRootId = ProductId.FromString("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"), Value = "Desk Techni" }
					);
				});

			mb.OwnsOne(
				pr => pr.Description,
				desc =>
				{
					desc.Property(x => x.Value).HasColumnName("Description");
					desc.HasData(
						new { ProductRootId = ProductId.FromString("de679c55-4c13-4fe7-91b4-69cbce3223a2"), Value = "Implement an ergonomic seating solution for your office with this maroon multipurpose chair. The included tilt tension knob lets you calibrate the tilt and recline resistance to your desired configuration, while the adjustable seat and armrests optimize your seating position for correct posture." },
						new { ProductRootId = ProductId.FromString("32f75bce-16a0-4070-9fac-4289678c191f"), Value = "The Lockland Big & Tall bonded leather managers chair offers top quality comfort, multiple adjustment features." },
						new { ProductRootId = ProductId.FromString("bcbc1851-6317-4022-be62-53d29c04bcda"), Value = "Carve out a personal workspace with this storage desk. The simple design and classic mid-century modern details make this desk perfect for modern decor themes or casual open office settings, and the rectangular desktop provides space for a laptop and peripherals." },
						new { ProductRootId = ProductId.FromString("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"), Value = "The Techni Mobili Complete Workstation Desk is everything you need in a computer desk and stay organized." }
					);
				});

			mb.OwnsOne(
				pr => pr.Price,
				price =>
				{
					price.Property(p => p.Amount).HasColumnName("Price");
					price.Property(p => p.Currency).HasColumnName("Price_Currency");
					price.HasData(
						new { ProductRootId = ProductId.FromString("de679c55-4c13-4fe7-91b4-69cbce3223a2"), Amount = 169m, Currency = "EUR" },
						new { ProductRootId = ProductId.FromString("32f75bce-16a0-4070-9fac-4289678c191f"), Amount = 263m, Currency = "EUR" },
						new { ProductRootId = ProductId.FromString("bcbc1851-6317-4022-be62-53d29c04bcda"), Amount = 305m, Currency = "EUR" },
						new { ProductRootId = ProductId.FromString("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"), Amount = 295m, Currency = "EUR" }
					);
				});
		}
	}
}