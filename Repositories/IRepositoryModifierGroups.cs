using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryModifierGroups:IRepositoryBase
    {
        Task<int> Create(ModifierGroup modifierGroup);

        Task<List<ModifierGroup>> GetAll(PaginationDTO paginationDTO);
        Task<ModifierGroup?> GetById(int id);

        Task<bool> IfExists(int id);

        Task Update(ModifierGroup modifierGroup);

        Task Delete(int id);
        Task<List<ModifierGroup>> GetByName(string name);
        Task<List<ModifierGroup>> GetByGroupOptionsName(string name);
        Task<List<int>> IfTheyExist(List<int> ids);
        Task AssignModifierOption(int id, List<ModifierOption> modifierOptions);
    }
}
