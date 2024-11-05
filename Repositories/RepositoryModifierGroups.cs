using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryModifierGroups : IRepositoryModifierGroups
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositoryModifierGroups(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<int> Create(ModifierGroup modifierGroup)
        {
            context.Add(modifierGroup);
            await context.SaveChangesAsync();
            return modifierGroup.Id;
        }

        public async Task Delete(int id)
        {
            await context.ModifierGroups.AsNoTracking().Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<ModifierGroup>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.ModifierGroups.AsQueryable();
            await httpContext.InsertParametersPaginationHeader(queryable);
            return await queryable.AsNoTracking().OrderBy(x => x.Name).Pagination(paginationDTO).ToListAsync();
        }

        public async Task<ModifierGroup?> GetById(int id)
        {
            return await context.ModifierGroups.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IfExists(int id)
        {
            return await context.ModifierGroups.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task Update(ModifierGroup modifierGroup)
        {
            context.Update(modifierGroup);
            await context.SaveChangesAsync();
        }

        public async Task<List<ModifierGroup>> GetByName(string name)
        {
            return await context.ModifierGroups.Where(a => a.Name.Contains(name)).OrderBy(a => a.Name).ToListAsync();
        }
    }
}
