using MenuOnlineUdemy.Entities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Image>().Property(p => p.File).IsUnicode();

            modelBuilder.Entity<ProductImage>().HasKey(g => new { g.ProductId, g.ImageId });
            modelBuilder.Entity<ProductModifierGroup>().HasKey(a => new { a.ProductId, a.ModifierGroupId });
            modelBuilder.Entity<ProductCategory>().HasKey(a => new { a.ProductId, a.CategoryId });
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<ModifierOption> ModifierOptions { get; set; }
        public DbSet<ModifierGroup> ModifierGroups { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductModifierGroup> ProductModifierGroups { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
