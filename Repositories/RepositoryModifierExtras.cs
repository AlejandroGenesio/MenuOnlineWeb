using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryModifierExtras : IRepositoryModifierExtras
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositoryModifierExtras(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<int> Create(ModifierExtra modifierExtra)
        {
            context.Add(modifierExtra);
            await context.SaveChangesAsync();
            return modifierExtra.Id;
        }

        public async Task Delete(int id)
        {
            await context.ModifierExtras.AsNoTracking().Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<ModifierExtra>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.ModifierExtras.AsQueryable();
            await httpContext.InsertParametersPaginationHeader(queryable);
            return await queryable.AsNoTracking().OrderBy(x => x.Name).Pagination(paginationDTO).ToListAsync();
            //return await context.ModifierExtras.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<ModifierExtra?> GetById(int id)
        {
            return await context.ModifierExtras.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IfExists(int id)
        {
            return await context.ModifierExtras.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task Update(ModifierExtra modifierExtra)
        {
            context.Update(modifierExtra);
            await context.SaveChangesAsync();
        }

        public async Task<List<ModifierExtra>> GetByName(string name)
        {
            return await context.ModifierExtras.Where(a => a.Name.Contains(name)).OrderBy(a => a.Name).ToListAsync();
        }
    }
}
