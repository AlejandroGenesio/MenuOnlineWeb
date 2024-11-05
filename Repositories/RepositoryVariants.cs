using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryVariants : IRepositoryVariants
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositoryVariants(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<int> Create(Variant variant)
        {
            context.Add(variant);
            await context.SaveChangesAsync();
            return variant.Id;
        }

        public async Task Delete(int id)
        {
            await context.Variants.AsNoTracking().Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Variant>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Variants.AsQueryable();
            await httpContext.InsertParametersPaginationHeader(queryable);
            return await queryable.AsNoTracking().OrderBy(x => x.Name).Pagination(paginationDTO).ToListAsync();
            //return await context.Variants.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Variant?> GetById(int id)
        {
            return await context.Variants.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IfExists(int id)
        {
            return await context.Variants.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task Update(Variant variant)
        {
            context.Update(variant);
            await context.SaveChangesAsync();
        }

        public async Task<List<Variant>> GetByName(string name)
        {
            return await context.Variants.Where(a => a.Name.Contains(name)).OrderBy(a => a.Name).ToListAsync();
        }
    }
}
