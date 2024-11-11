using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using MenuOnlineUdemy.services.Import;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MenuOnlineUdemy.Endpoints
{
    public static class CategoryEndpoints
    {
        public static RouteGroupBuilder MapCategories(this RouteGroupBuilder group)
        {

            group.MapGet("/", GetCategories);

            group.MapGet("/{id:int}", GetCategoriesById);

            group.MapPost("/", CreateCategory);

            group.MapPut("/{id:int}", UpdateCategory);

            group.MapDelete("/{id:int}", DeleteCategory);

            group.MapGet("/getbyname/{name}", GetCategoriesByName);

            return group;
        }

        static async Task<Ok<List<CategoryDTO>>> GetCategories(IRepositoryCategories repository, IMapper mapper,
            int page = 1, int recordsByPage = 10)
        {
            var pagination = new PaginationDTO { Page = page, RecordsByPage = recordsByPage };
            var categories = await repository.GetAll(pagination);
            var categoriesDTO = mapper.Map<List<CategoryDTO>>(categories); 
            return TypedResults.Ok(categoriesDTO);
        }

        static async Task<Ok<List<CategoryDTO>>> GetCategorysByName(string name, IRepositoryCategories repository, IMapper mapper)
        {

            var categories = await repository.GetByName(name);
            var categoriesDTO = mapper.Map<List<CategoryDTO>>(categories);
            return TypedResults.Ok(categoriesDTO);
        }

        static async Task<Results<Ok<CategoryDTO>, NotFound>> GetCategoriesById(IRepositoryCategories repository, int id
            , IMapper mapper)
        {
            var category = await repository.GetById(id);

            if (category == null)
            {
                return TypedResults.NotFound();
            }

            var categoryDTO = mapper.Map<CategoryDTO>(category);

            return TypedResults.Ok(categoryDTO);
        }

        static async Task<Created<CategoryDTO>> CreateCategory(CreateCategoryDTO createCategoryDTO, IRepositoryCategories repository
            , IMapper mapper)
        {
            var category = mapper.Map<Category>(createCategoryDTO);

            var id = await repository.Create(category);

            var categoryDTO = mapper.Map<CategoryDTO>(category);

            return TypedResults.Created($"/categories/{id}", categoryDTO);
        }

        static async Task<Results<NoContent, NotFound>> UpdateCategory(int id, CreateCategoryDTO createCategoryDTO, IRepositoryCategories repository
            , IMapper mapper)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            var category = mapper.Map<Category>(createCategoryDTO);
            category.Id = id;

            await repository.Update(category);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> DeleteCategory(int id, IRepositoryCategories repository)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            return TypedResults.NoContent();
        }

        static async Task<Ok<List<ProductDTO>>> GetCategoriesByName(string name, IRepositoryCategories repository, IMapper mapper)
        {

            var categories = await repository.GetByName(name);
            var categoriesDTO = mapper.Map<List<ProductDTO>>(categories); //products.Select(x => new  ProductDTO { Id = x.Id, Name = x.Name}).ToList();
            return TypedResults.Ok(categoriesDTO);
        }

    }
}
