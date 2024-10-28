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
            return await context.Products.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IfExists(int id)
        {
            return await context.Products.AnyAsync(x => x.Id == id);
        }

        public async Task Update(Product product)
        {
            context.Update(product);
            await context.SaveChangesAsync();
        }
    }
}
