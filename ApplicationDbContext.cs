using MenuOnlineUdemy.Entities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<ModifierExtras> ModifiersExtras { get; set; }
        public DbSet<ModifierGroup> ModifierGroups { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
