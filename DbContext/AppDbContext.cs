using Microsoft.EntityFrameworkCore;
using PetFeast_Backend2.Models.AddressModels;
using PetFeast_Backend2.Models.CartModels;
using PetFeast_Backend2.Models.CategoryModels;
using PetFeast_Backend2.Models.OrderModels;
using PetFeast_Backend2.Models.ProductModels;
using PetFeast_Backend2.Models.UserModels;
using PetFeast_Backend2.Models.WishListModels;

namespace PetFeast_Backend2
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;
        private readonly string _conStr;

        public AppDbContext(IConfiguration config)
        {
            _config = config;
            _conStr = _config["ConnectionStrings:DefaultConnection"];
        }

        // Represent tables in database
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<OrderMain> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_conStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Default value of User.Role
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("User");

            // Default value of User.IsBlocked
            modelBuilder.Entity<User>()
                .Property(u => u.IsBlocked)
                .HasDefaultValue(false);

            // Categrory & Product Relation
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(c => c.CategoryId);

            // User & Cart Relation
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId);

            // Cart & CartItem Relation 
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.cartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(c => c.CartId);

            // CartItem And Product Relation
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId);

            // WishList and User Relation
            modelBuilder.Entity<WishList>()
                .HasOne(w => w.User)
                .WithMany(u => u.WishLists)
                .HasForeignKey(u => u.UserId);

            // WishList and Product Relation
            modelBuilder.Entity<WishList>()
                .HasOne(w => w.Product)
                .WithMany()
                .HasForeignKey(w => w.ProductId);
            
            // Order and User Relation
            modelBuilder.Entity<OrderMain>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            // OrderItem and Order Relation
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            // OrderItem and Product Relation
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);


            // Default value of MRP
            modelBuilder.Entity<Product>()
                .Property(p => p.MRP)
                .HasDefaultValue(100);

            // Default value of Stock
            modelBuilder.Entity<Product>()
                .Property(p => p.Stock)
                .HasDefaultValue(10);

            // Address and User Relation
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);

            // OrderMain and Address Relatioin
            modelBuilder.Entity<OrderMain>()
                .HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // Default value of OrderStatus
            modelBuilder.Entity<OrderMain>()
                .Property(o => o.OrderStatus)
                .HasDefaultValue("Pending");


            base.OnModelCreating(modelBuilder);
        }
    }
}
