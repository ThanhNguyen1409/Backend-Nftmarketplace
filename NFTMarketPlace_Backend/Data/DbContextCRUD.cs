using Microsoft.EntityFrameworkCore;
using NFTMarketPlace_Backend.Models;

namespace NFTMarketPlace_Backend.Data
{
    public class DbContextCRUD : DbContext
    {
        public DbContextCRUD(DbContextOptions<DbContextCRUD> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Account>()
            //    .Property(p => p.RefreshTokenExpiryTime)
            //    .HasColumnType("datetime2(0)"); // Đặt kiểu dữ liệu trong SQL Server là datetime2(0)
            modelBuilder.Entity<NFTTransaction>()
                   .Property(p => p.Date)
                  .HasColumnType("datetime2(0)");

            //modelBuilder.Entity<Rating>()
            //       .Property(p => p.ratingDate)
            //       .HasColumnType("datetime2(0)");
        }

        //public DbSet<Product> Products { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<Customer> Customers { get; set; }

        //public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Collection> Collections { get; set; }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<NFTTransaction> nFTTransactions { get; set; }
        public DbSet<NFT> nFTs { get; set; }
        public DbSet<Offer> offers { get; set; }
        //public DbSet<AccountRoles> AccountRoles { get; set; }

        //public DbSet<Role> Roles { get; set; }

        //public DbSet<Rating> Ratings { get; set; }
        //public DbSet<Size> Sizes { get; set; }
        //public DbSet<Notification> Notifications { get; set; }
    }


}
