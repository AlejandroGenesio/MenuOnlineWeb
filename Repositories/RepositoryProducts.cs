using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryProducts : IRepositoryProducts
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly HttpContext httpContext;

        public RepositoryProducts(ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            httpContext = httpContextAccessor.HttpContext!;
        }
        public async Task<int> Create(Product product)
        {
            context.Add(product);
            await context.SaveChangesAsync();
            return product.Id;
        }

        public async Task Delete(int id)
        {
            await context.Products.AsNoTracking().Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Product>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Products.AsQueryable();
            await httpContext.InsertParametersPaginationHeader(queryable);
            return await queryable.AsNoTracking().OrderBy(x => x.Name).Pagination(paginationDTO).ToListAsync();
            //return await context.Products.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductImages.OrderBy(a => a.Order))
                    .ThenInclude(ap => ap.Image)
                .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IfExists(int id)
        {
            return await context.Products.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task Update(Product product)
        {
            context.Update(product);
            await context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetByName(string name)
        {
            return await context.Products.Where(a => a.Name.Contains(name)).OrderBy(a => a.Name).ToListAsync();
        }

        public async Task AssignImages(int id, List<int> imagesIds)
        {
            var product = await context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                throw new ArgumentException($"The id {id} does not exist for this context");
            }

            var productImages = imagesIds.Select(imageId => new ProductImage() { ImageId = imageId });

            product.ProductImages = mapper.Map(productImages, product.ProductImages);

            await context.SaveChangesAsync();
        }

        public async Task AssignCategories(int id, List<int> categoriesIds)
        {
            var product = await context.Products.Include(p => p.ProductCategories).FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                throw new ArgumentException($"The id {id} does not exist for this context");
            }

            var productCategories = categoriesIds.Select(categoryId => new ProductCategory() { CategoryId = categoryId });

            product.ProductCategories = mapper.Map(productCategories, product.ProductCategories);

            await context.SaveChangesAsync();
        }

        public async Task AssignModifierGroup(int id, List<ProductModifierGroup> modifierGroups)
        {
            var product = await context.Products.Include(x => x.ProductModifierGroups).FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                throw new ArgumentException($"Id: {id} does not exist.");
            }

            product.ProductModifierGroups = mapper.Map(modifierGroups, product.ProductModifierGroups);

            await context.SaveChangesAsync();
        }

        public void DiscardChanges()
        {
            throw new NotImplementedException();
        }

        public bool IsEmptyId(int value)
        {
            return value == 0;
        }
    }
}
