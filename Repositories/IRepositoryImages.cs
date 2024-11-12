using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryImages
    {
        Task Update(Image image);
        Task Delete(int id);
        Task<int> Create(Image image);
        Task<bool> IfExists(int id);
        Task<List<Image>> GetAll();
        Task<Image?> GetById(int id);
        Task<List<int>> IfTHeyExist(List<int> ids);
    }
}
