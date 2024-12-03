using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryOrders : IRepositoryOrders
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositoryOrders(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<int> Create(Order order)
        {
            context.Add(order);
            await context.SaveChangesAsync();
            return order.Id;
        }

        public async Task Delete(int id)
        {
            await context.Orders.AsNoTracking().Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Order>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Orders.AsQueryable();
            await httpContext.InsertParametersPaginationHeader(queryable);
            return await queryable
                .Include(p => p.OrderDetails)
                .AsNoTracking()
                .OrderByDescending(x => x.timestamp)
                .Pagination(paginationDTO).ToListAsync();
        }

        public async Task<Order?> GetById(int id)
        {
            return await context.Orders
                .Include(p => p.OrderDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IfExists(int id)
        {
            return await context.Orders.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task Update(Order order)
        {
            context.Update(order);
            await context.SaveChangesAsync();
        }
    }
}
