using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryCategories : IRepositoryCategories
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly HttpContext httpContext;

        public RepositoryCategories(ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<int> Create(Category category)
        {
            context.Add(category);
            await context.SaveChangesAsync();
            return category.Id;
        }

        public async Task Delete(int id)
        {
            await context.Categories.AsNoTracking().Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Category>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Categories.AsQueryable();
            await httpContext.InsertParametersPaginationHeader(queryable);
            return await queryable.AsNoTracking().OrderBy(x => x.Name).Pagination(paginationDTO).ToListAsync();
        }

        public async Task<Category?> GetById(int id)
        {
            return await context.Categories.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IfExists(int id)
        {
            return await context.Categories.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task Update(Category category)
        {
            context.Update(category);
            await context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetByName(string name)
        {
            return await context.Categories.Where(a => a.Name.Contains(name)).OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<List<int>> IfTheyExist(List<int> ids)
        {
            return await context.Categories.Where(g => ids.Contains(g.Id)).Select(g => g.Id).ToListAsync();
        }

    }
}
