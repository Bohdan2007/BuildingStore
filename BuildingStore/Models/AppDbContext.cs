using Microsoft.EntityFrameworkCore;

namespace BuildingStore.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(o => o.OrderStatus).HasConversion<string>();
            modelBuilder.Entity<OrderItem>().Property(o => o.ProductStatus).HasConversion<string>();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>().HasOne(o => o.Admin).WithMany().HasForeignKey(o => o.AdminId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
