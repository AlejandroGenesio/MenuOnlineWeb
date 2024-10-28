using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryImages
    {
        Task<int> Create(Image image);
    }
}
