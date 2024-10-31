using Microsoft.EntityFrameworkCore;

namespace MenuOnlineUdemy.Utilities
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParametersPaginationHeader<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            double quantity = await queryable.CountAsync();
            httpContext.Response.Headers.Append("totalRecords", quantity.ToString());
        }
    }
}
