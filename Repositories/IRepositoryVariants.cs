using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryVariants: IRepositoryBase
    {
        Task<int> Create(Variant variant);

        Task<List<Variant>> GetAll(int productId, PaginationDTO paginationDTO);
        Task<Variant?> GetById(int id);

        Task<bool> IfExists(int id);

        Task Update(Variant variant);

        Task Delete(int id);
        Task<List<Variant>> GetByName(string name);
    }
}
