using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryProducts
    {
        Task<int> Create(Product product);

        Task<List<Product>> GetAll();
        Task<Product?> GetById(int id);

        Task<bool> IfExists(int id);

        Task Update(Product product);
    }
}
