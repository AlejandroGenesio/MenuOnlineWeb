using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryModifierExtras
    {
        Task<int> Create(ModifierExtra modifierExtra);

        Task<List<ModifierExtra>> GetAll(PaginationDTO paginationDTO);
        Task<ModifierExtra?> GetById(int id);

        Task<bool> IfExists(int id);

        Task Update(ModifierExtra modifierExtra);

        Task Delete(int id);
        Task<List<ModifierExtra>> GetByName(string name);
    }
}
