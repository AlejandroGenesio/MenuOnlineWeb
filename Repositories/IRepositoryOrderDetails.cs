using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryOrderDetails
    {
        Task<int> Create(OrderDetails orderDetails);
    }
}
