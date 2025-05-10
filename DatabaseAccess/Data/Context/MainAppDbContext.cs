using DatabaseAccess.Data.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess.Data.Context
{
	public class MainAppDbContext : DbContext
	{
		public MainAppDbContext(DbContextOptions<MainAppDbContext> options)
			: base(options)
		{
		}

		public MainAppDbContext()
		{ }

		// NOTE: virtual is needed in order to support unit testing
		public virtual DbSet<AspNetUserDAO> AspNetUsers { get; set; }

		public virtual DbSet<AspNetRoleDAO> AspNetRoles { get; set; }

		public virtual DbSet<AspNetUserRoleDAO> AspNetUserRoles { get; set; }

		public virtual DbSet<AspNetUserClaimDAO> AspNetUserClaims { get; set; }

		public virtual DbSet<OrderDAO> Orders { get; set; }

		public virtual DbSet<OrderDetailDAO> OrderDetails { get; set; }

		public virtual DbSet<MenuListingDAO> MenuListings { get; set; }

		public virtual DbSet<ShoppingCartDAO> ShoppingCarts { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Example of composite key; only valid using Fluent API
			modelBuilder.Entity<OrderDetailDAO>()
				.HasKey(c => new { c.OrderId, c.OrderDetailId })
				.HasName("PK_OrderIdOrderDetailId");

			modelBuilder.Entity<AspNetUserRoleDAO>()
				.HasKey(c => new { c.UserId, c.RoleId })
				.HasName("PK_AspNetUserRoles"); ;
		}

	}
}