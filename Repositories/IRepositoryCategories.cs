using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryCategories
    {
        Task<int> Create(Category category);

        Task<List<Category>> GetAll(PaginationDTO paginationDTO);
        Task<Category?> GetById(int id);

        Task<bool> IfExists(int id);

        Task Update(Category category);

        Task Delete(int id);
        Task<List<Category>> GetByName(string name);
    }
}
