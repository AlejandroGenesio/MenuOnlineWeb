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

        static async Task<Ok<List<VariantDTO>>> GetVariants(IRepositoryVariants repository, IMapper mapper,
            int page = 1, int recordsByPage = 10)
        {
            var pagination = new PaginationDTO { Page = page, RecordsByPage = recordsByPage };
            var variants = await repository.GetAll(pagination);
            var variantsDTO = mapper.Map<List<VariantDTO>>(variants); //variants.Select(x => new  VariantDTO { Id = x.Id, Name = x.Name}).ToList();
            return TypedResults.Ok(variantsDTO);
        }

        static async Task<Ok<List<VariantDTO>>> GetVariantsByName(string name, IRepositoryVariants repository, IMapper mapper)
        {

            var variants = await repository.GetByName(name);
            var variantsDTO = mapper.Map<List<VariantDTO>>(variants); //variants.Select(x => new  VariantDTO { Id = x.Id, Name = x.Name}).ToList();
            return TypedResults.Ok(variantsDTO);
        }

        static async Task<Results<Ok<VariantDTO>, NotFound>> GetVariantsById(IRepositoryVariants repository, int id
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

        static async Task<Created<VariantDTO>> CreateVariant(CreateVariantDTO createVariantDTO, IRepositoryVariants repository
            , IMapper mapper)
        {
            var variant = mapper.Map<Variant> (createVariantDTO);

            var id = await repository.Create(variant);

            var variantDTO = mapper.Map<VariantDTO>(variant);

            return TypedResults.Created($"/variants/{id}", variantDTO);
        }

        static async Task<Results<NoContent, NotFound>> UpdateVariant(int id, CreateVariantDTO createVariantDTO, IRepositoryVariants repository
            , IMapper mapper)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            var variant = mapper.Map<Variant>(createVariantDTO);
            variant.Id = id;

            await repository.Update(variant);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> DeleteVariant(int id, IRepositoryVariants repository)
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
