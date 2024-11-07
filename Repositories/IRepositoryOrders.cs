using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryOrders
    {
        Task<int> Create(Order order);

        Task<List<Order>> GetAll(PaginationDTO paginationDTO);
        Task<Order?> GetById(int id);

        Task<bool> IfExists(int id);

        Task Update(Order order);

        Task Delete(int id);
    }
}
