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
    }
}
