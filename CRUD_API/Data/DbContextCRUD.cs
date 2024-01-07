using CRUD_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_API.Data
{
    public class DbContextCRUD : DbContext
    {
        public DbContextCRUD(DbContextOptions<DbContextCRUD> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .Property(p => p.RefreshTokenExpiryTime)
                .HasColumnType("datetime2(0)"); // Đặt kiểu dữ liệu trong SQL Server là datetime2(0)
            modelBuilder.Entity<Order>()
                   .Property(p => p.orderDate)
                   .HasColumnType("datetime2(0)");
            modelBuilder.Entity<Rating>()
                   .Property(p => p.ratingDate)
                   .HasColumnType("datetime2(0)");
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountRoles> AccountRoles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Rating> Ratings { get; set; }
    }


}
