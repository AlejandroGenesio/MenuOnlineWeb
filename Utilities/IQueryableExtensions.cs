﻿using MenuOnlineUdemy.DTOs;

namespace MenuOnlineUdemy.Utilities
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Pagination<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page-1)*paginationDTO.RecordsByPage)
                .Take(paginationDTO.RecordsByPage);
        }
    }
}
