using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryModifierGroups : IRepositoryModifierGroups
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly HttpContext httpContext;

        public RepositoryModifierGroups(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor
            , IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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
            return await queryable.AsNoTracking()
                .Include(p => p.ModifierGroupOptions)
                .OrderBy(x => x.Label)
                .Pagination(paginationDTO)
                .ToListAsync();
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
            return await context.ModifierGroups.Where(a => a.Label.Contains(name)).OrderBy(a => a.Label).ToListAsync();
        }

        public async Task<List<int>> IfTheyExist(List<int> ids)
        {
            return await context.Products.Where(a => ids.Contains(a.Id)).Select(a => a.Id).ToListAsync();
        }

        public bool IsEmptyId(int value)
        {
            return value == 0;
        }

        public async Task AssignModifierOption(int id, List<ModifierOption> modifierOptions)
        {
            var modifierGroup = await context.ModifierGroups.Include(x => x.ModifierGroupOptions).FirstOrDefaultAsync(p => p.Id == id);

            if (modifierGroup is null)
            {
                throw new ArgumentException($"Id: {id} does not exist.");
            }

            modifierGroup.ModifierGroupOptions = modifierOptions;

            await context.SaveChangesAsync();
        }

        public  Task<List<ModifierGroup>> GetByGroupOptionsName(string optionsGroupName)
        {
            return  context.ModifierGroups.Where(p => p.OptionsGroup == optionsGroupName).ToListAsync();
        }
    }
}
