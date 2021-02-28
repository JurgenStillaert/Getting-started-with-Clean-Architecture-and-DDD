using Microsoft.EntityFrameworkCore;

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

			//Seeding data is removed from this branch
		}
	}
}