using MenuOnlineUdemy.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<ModifierExtra> ModifierExtras { get; set; }
        public DbSet<ModifierGroup> ModifierGroups { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
