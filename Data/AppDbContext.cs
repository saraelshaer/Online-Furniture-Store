using FurnitureStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FurnitureStore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                 .HasKey(k => new { k.ProductId, k.OrderId });

            modelBuilder.Entity<CartProduct>()
                .HasKey(k => new { k.ProductId, k.CartId });

            modelBuilder.Entity<User>(config =>
            {
                config.Property(c => c.IsActive)
                   .HasDefaultValue(true);

                config.Property(c => c.ImageFileName)
                .HasDefaultValue("defaultUserImage.jpg");
            });
             

            modelBuilder.Entity<Review>()
              .Property(c => c.IsActive)
              .HasDefaultValue(true);

            modelBuilder.Entity<Category>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Product>(config =>
            {
                config.Property(c => c.IsActive)
                .HasDefaultValue(true);

                config.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Order>()
                .Property(p => p.OrderStatus)
                .HasConversion<string>();


            modelBuilder.Entity<Role>()
              .HasMany(r=>r.UserRoles)
              .WithOne(r => r.Role)
              .OnDelete(DeleteBehavior.SetNull);
               

            modelBuilder.Entity<User>()
                .HasData
                (
                  new User
                  {
                      Id = 1,
                      FirstName = "Admin1",
                      LastName = "AdminLastName",
                      Email = "admin1@gmail.com",
                      Password = "Admin123456??",
                      Phone = "+201235444441",
                      Country = "Egypt",
                      State = "",
                      City = "",
                      ZipCode = "1234"
                  },
                  new User
                  {
                      Id = 2,
                      FirstName = "Admin2",
                      LastName = "AdminLastName",
                      Email = "admin2@gmail.com",
                      Password = "Admin123456??",
                      Phone = "+201235444441",
                      Country = "Egypt",
                      State = "",
                      City = "",
                      ZipCode = "1234"
                  }

                );

            modelBuilder.Entity<Role>()
                .HasData
                (
                  new Role
                  {
                      Id = 1,
                      Name = "Admin"
                  },

                  new Role
                  {
                      Id = 2,
                      Name = "Regular User"
                  }
                );

            modelBuilder.Entity<UserRole>()
               .HasData
               (
                 new UserRole
                 {
                     Id = 1,
                     UserId = 1,
                     RoleId = 1,
                 },
                 new UserRole
                 {
                     Id = 2,
                     UserId = 2,
                     RoleId = 1,
                 }

               );


            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review>Reviews { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListProduct> WishListProducts { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

    }
}
