using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryProducts : IRepositoryProducts
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositoryProducts(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<int> Create(Product product)
        {
            context.Add(product);
            await context.SaveChangesAsync();
            return product.Id;
        }

        public async Task Delete(int id)
        {
            await context.Products.AsNoTracking().Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Product>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Products.AsQueryable();
            await httpContext.InsertParametersPaginationHeader(queryable);
            return await queryable.AsNoTracking().OrderBy(x => x.Name).Pagination(paginationDTO).ToListAsync();
            //return await context.Products.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IfExists(int id)
        {
            return await context.Products.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task Update(Product product)
        {
            context.Update(product);
            await context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetByName(string name)
        {
            return await context.Products.Where(a => a.Name.Contains(name)).OrderBy(a => a.Name).ToListAsync();
        }
    }
}
