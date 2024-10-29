using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.CompilerServices;

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

            return group;
        }

        static async Task<Ok<List<Product>>> GetProducts(IRepositoryProducts repository)
        {

            var products = await repository.GetAll();
            return TypedResults.Ok(products);
        }

        static async Task<Results<Ok<Product>, NotFound>> GetProductsById(IRepositoryProducts repository, int id)
        {
            var product = await repository.GetById(id);

            if (product == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(product);
        }

        static async Task<Created<Product>> CreateProduct(Product product, IRepositoryProducts repository)
        {
            var id = await repository.Create(product);
            return TypedResults.Created($"/products/{id}", product);
        }

        static async Task<Results<NoContent, NotFound>> UpdateProduct(int id, Product product, IRepositoryProducts repository)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

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
