﻿using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryVariants
    {
        Task<int> Create(Variant variant);

        Task<List<Variant>> GetAll(PaginationDTO paginationDTO);
        Task<Variant?> GetById(int id);

        Task<bool> IfExists(int id);

        Task Update(Variant variant);

        Task Delete(int id);
        Task<List<Variant>> GetByName(string name);
    }
}