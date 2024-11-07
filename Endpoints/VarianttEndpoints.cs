using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MenuOnlineUdemy.Endpoints
{
    public static class VariantEndpoints
    {
        public static RouteGroupBuilder MapVariants(this RouteGroupBuilder group)
        {

            group.MapGet("/", GetVariants);

            group.MapGet("/{id:int}", GetVariantsById);

            group.MapPost("/", CreateVariant);

            group.MapPut("/{id:int}", UpdateVariant);

            group.MapDelete("/{id:int}", DeleteVariant);

            group.MapGet("/getbyname/{name}", GetVariantsByName);

            return group;
        }

        static async Task<Results<Ok<List<VariantDTO>>, NotFound>> GetVariants(int productId
            , IRepositoryVariants repositoryVariants, IRepositoryProducts repositoryProducts
            , IMapper mapper,
            int page = 1, int recordsByPage = 10)
        {
            if (!await repositoryProducts.IfExists(productId))
            {
                return TypedResults.NotFound();
            }

            var pagination = new PaginationDTO { Page = page, RecordsByPage = recordsByPage };
            var variants = await repositoryVariants.GetAll(productId, pagination);
            var variantsDTO = mapper.Map<List<VariantDTO>>(variants); //variants.Select(x => new  VariantDTO { Id = x.Id, Name = x.Name}).ToList();
            return TypedResults.Ok(variantsDTO);
        }

        static async Task<Ok<List<VariantDTO>>> GetVariantsByName(string name, IRepositoryVariants repository, IMapper mapper)
        {

            var variants = await repository.GetByName(name);
            var variantsDTO = mapper.Map<List<VariantDTO>>(variants); //variants.Select(x => new  VariantDTO { Id = x.Id, Name = x.Name}).ToList();
            return TypedResults.Ok(variantsDTO);
        }

        static async Task<Results<Ok<VariantDTO>, NotFound>> GetVariantsById(int productId
            , IRepositoryVariants repository
            , int id
            , IMapper mapper)
        {
            var variant = await repository.GetById(id);

            if (variant == null)
            {
                return TypedResults.NotFound();
            }

            var variantDTO = mapper.Map<VariantDTO>(variant);

            return TypedResults.Ok(variantDTO);
        }

        static async Task<Results<Created<VariantDTO>, NotFound>> CreateVariant(int productId, CreateVariantDTO createVariantDTO
            , IRepositoryVariants repositoryVariants
            , IRepositoryProducts repositoryProducts, IMapper mapper)
        {
            if(! await repositoryProducts.IfExists(productId))
            {
                return TypedResults.NotFound();
            }

            var variant = mapper.Map<Variant> (createVariantDTO);
            variant.ProductId = productId;

            var id = await repositoryVariants.Create(variant);

            var variantDTO = mapper.Map<VariantDTO>(variant);

            return TypedResults.Created($"/variants/{id}", variantDTO);
        }

        static async Task<Results<NoContent, NotFound>> UpdateVariant(int productId, int id
            , CreateVariantDTO createVariantDTO, IRepositoryVariants repositoryVariants
            , IRepositoryProducts repositoryProducts
            , IMapper mapper)
        {
            if(!await repositoryProducts.IfExists(productId))
            {
                return TypedResults.NotFound();
            }

            if (!await repositoryVariants.IfExists(id))
            {
                return TypedResults.NotFound();
            }

            var variant = mapper.Map<Variant>(createVariantDTO);
            variant.Id = id;
            variant.ProductId = productId;

            await repositoryVariants.Update(variant);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> DeleteVariant(int productId, int id
            , IRepositoryVariants repositoryVariants
            , IRepositoryProducts repositoryProducts)
        {

            if (!await repositoryVariants.IfExists(id))
            {
                return TypedResults.NotFound();
            }

            await repositoryVariants.Delete(id);
            return TypedResults.NoContent();
        }

    }
}
