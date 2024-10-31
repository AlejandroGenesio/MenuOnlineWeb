using MenuOnlineUdemy.Entities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryImages : IRepositoryImages
    {
        private readonly ApplicationDbContext context;

        public RepositoryImages(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Image>> GetAll()
        {
            return await context.Images.AsNoTracking().OrderBy(a => a.File).ToListAsync();
        }

        public async Task<Image?> GetById(int id)
        {
            return await context.Images.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> Create(Image image)
        {
            context.Add(image);

            await context.SaveChangesAsync();
            return image.Id;
        }

        public async Task<bool> IfExists(int id)
        {
            return await context.Images.AsNoTracking().AnyAsync(a => a.Id == id);
        }

        public async Task Update(Image image)
        {
            context.Update(image);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await context.Images.AsNoTracking().Where(a => a.Id == id).ExecuteDeleteAsync();
        }

    }
}
