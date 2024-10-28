using MenuOnlineUdemy.Entities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryProducts : IRepositoryProducts
    {
        private readonly ApplicationDbContext context;

        public RepositoryProducts(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<int> Create(Product product)
        {
            context.Add(product);
            await context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<List<Product>> GetAll()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
