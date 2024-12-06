using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryProducts : IRepositoryBase
    {
        Task<int> Create(Product product);

        Task<List<Product>> GetAll(PaginationDTO paginationDTO);
        Task<Product?> GetById(int id);

        Task<bool> IfExists(int id);

        Task Update(Product product);

        Task Delete(int id);
        Task<List<Product>> GetByName(string name);

        Task<Product?> FindByName(string name);
        Task AssignImages(int id, List<int> imagesIds);
        Task AssignModifierGroup(int id, List<int> modifierGroups);

        void DiscardChanges();        
        bool IsEmptyId(int value);
        Task AssignCategories(int id, List<int> categoriesIds);
        Task<bool> DuplicationExists(int id, string name);
    }
}
