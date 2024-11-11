using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryModifierOptions
    {
        Task<int> Create(ModifierOption modifierOption);

        Task<List<ModifierOption>> GetAll(PaginationDTO paginationDTO);
        Task<ModifierOption?> GetById(int id);

        Task<bool> IfExists(int id);

        Task Update(ModifierOption modifierOption);

        Task Delete(int id);
        Task<List<ModifierOption>> GetByName(string name);
    }
}
