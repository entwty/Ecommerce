using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data
{
    public class DataContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductRating> ProductRatings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<ProductImage> Images { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ürün ve Kategori ilişkisi
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ürün ve Satıcı ilişkisi
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ürün ve Resim ilişkisi
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne()
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Sepet öğesi ve Ürün ilişkisi
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });


                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles").HasKey(r => r.Id);
            modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.Id);
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = Guid.NewGuid(), Name = "Customer", NormalizedName = "CUSTOMER" },
                new Role { Id = Guid.NewGuid(), Name = "Seller", NormalizedName = "SELLER" },
                new Role { Id = Guid.NewGuid(), Name = "Courier", NormalizedName = "COURIER" }
                // Diğer rolleri ekleyin
            );

            SeedAdminUser(modelBuilder);
        }

        private void SeedAdminUser(ModelBuilder modelBuilder)
        {
            var adminRoleId = Guid.NewGuid();
            var adminUserId = Guid.NewGuid();

            var hasher = new PasswordHasher<User>();

            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminUserId,
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "YourAdminPasswordHere")
                // Diğer Admin kullanıcı özellikleri
            });

            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });
        }
    }
}