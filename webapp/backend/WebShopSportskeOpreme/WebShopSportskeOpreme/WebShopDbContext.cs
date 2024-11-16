using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme
{
    public class WebShopDbContext : DbContext
    {

        public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionProduct> PromotionsProducts { get;set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set;}
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<CustomerQuestion> CustomerQuestions { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreSupplier> StoreSuppliers { get; set; }
        public DbSet<StoreImage> StoreImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<StoreSupplier>()
                .HasKey(ss => new { ss.StoreId, ss.SupplierId });

            modelBuilder.Entity<StoreSupplier>()
                .HasOne(ss => ss.Store)
                .WithMany(s => s.StoreSuppliers)
                .HasForeignKey(ss => ss.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StoreSupplier>()
                .HasOne(ss => ss.Supplier)
                .WithMany(s => s.StoreSuppliers)
                .HasForeignKey(ss => ss.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
