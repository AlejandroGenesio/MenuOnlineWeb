using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MenuOnlineUdemy.Endpoints
{
    public static class ProductEndpoints
    {
        public static RouteGroupBuilder MapProducts(this RouteGroupBuilder group)
        {

            group.MapGet("/", GetProducts);

            group.MapGet("/{id:int}", GetProductsById);

            group.MapPost("/", CreateProduct);

            group.MapPut("/{id:int}", UpdateProduct);

            group.MapDelete("/{id:int}", DeleteProduct);

            group.MapGet("/getbyname/{name}", GetProductsByName);

            return group;
        }

        static async Task<Ok<List<ProductDTO>>> GetProducts(IRepositoryProducts repository, IMapper mapper,
            int page = 1, int recordsByPage = 10)
        {
            var pagination = new PaginationDTO { Page = page, RecordsByPage = recordsByPage };
            var products = await repository.GetAll(pagination);
            var productsDTO = mapper.Map<List<ProductDTO>>(products); //products.Select(x => new  ProductDTO { Id = x.Id, Name = x.Name}).ToList();
            return TypedResults.Ok(productsDTO);
        }

        static async Task<Ok<List<ProductDTO>>> GetProductsByName(string name, IRepositoryProducts repository, IMapper mapper)
        {

            var products = await repository.GetByName(name);
            var productsDTO = mapper.Map<List<ProductDTO>>(products); //products.Select(x => new  ProductDTO { Id = x.Id, Name = x.Name}).ToList();
            return TypedResults.Ok(productsDTO);
        }

        static async Task<Results<Ok<ProductDTO>, NotFound>> GetProductsById(IRepositoryProducts repository, int id
            , IMapper mapper)
        {
            var product = await repository.GetById(id);

            if (product == null)
            {
                return TypedResults.NotFound();
            }

            var productDTO = mapper.Map<ProductDTO>(product);

            return TypedResults.Ok(productDTO);
        }

        static async Task<Created<ProductDTO>> CreateProduct(CreateProductDTO createProductDTO, IRepositoryProducts repository
            , IMapper mapper)
        {
            var product = mapper.Map<Product> (createProductDTO);

            var id = await repository.Create(product);

            var productDTO = mapper.Map<ProductDTO>(product);

            return TypedResults.Created($"/products/{id}", productDTO);
        }

        static async Task<Results<NoContent, NotFound>> UpdateProduct(int id, CreateProductDTO createProductDTO, IRepositoryProducts repository
            , IMapper mapper)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            var product = mapper.Map<Product>(createProductDTO);
            product.Id = id;

            await repository.Update(product);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> DeleteProduct(int id, IRepositoryProducts repository)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            return TypedResults.NoContent();
        }

    }
}
