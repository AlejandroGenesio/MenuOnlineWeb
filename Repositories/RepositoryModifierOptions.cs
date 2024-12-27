using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryModifierOptions : IRepositoryModifierOptions
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositoryModifierOptions(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<int> Create(ModifierOption modifierOption)
        {
            context.Add(modifierOption);
            await context.SaveChangesAsync();
            return modifierOption.Id;
        }

        public async Task Delete(int id)
        {
            await context.ModifierOptions.AsNoTracking().Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<ModifierOption>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.ModifierOptions.AsQueryable();
            await httpContext.InsertParametersPaginationHeader(queryable);
            return await queryable.AsNoTracking().OrderBy(x => x.Name).Pagination(paginationDTO).ToListAsync();
            //return await context.ModifierOptions.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<ModifierOption?> GetById(int id)
        {
            return await context.ModifierOptions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IfExists(int id)
        {
            return await context.ModifierOptions.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task Update(ModifierOption modifierOption)
        {
            context.Update(modifierOption);
            await context.SaveChangesAsync();
        }

        public async Task<List<ModifierOption>> GetByName(string name)
        {
            return await context.ModifierOptions.Where(a => a.Name.Contains(name)).OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<List<int>> IfTheyExists(List<int> ids)
        {
            return await context.ModifierOptions.Where(g => ids.Contains(g.Id)).Select(g => g.Id).ToListAsync();
        }

    }
}
