using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Repositories
{
    public class RepositoryOrderDetails : IRepositoryOrderDetails
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositoryOrderDetails(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<int> Create(OrderDetails orderDetails)
        {
            context.Add(orderDetails);
            await context.SaveChangesAsync();
            return orderDetails.Id;
        }
    }
}
