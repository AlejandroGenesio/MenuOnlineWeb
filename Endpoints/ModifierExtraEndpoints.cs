using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MenuOnlineUdemy.Endpoints
{
    public static class ModifierExtraEndpoints
    {
        public static RouteGroupBuilder MapModifierExtras(this RouteGroupBuilder group)
        {

            group.MapGet("/", GetModifierExtras);

            group.MapGet("/{id:int}", GetModifierExtrasById);

            group.MapPost("/", CreateModifierExtra);

            group.MapPut("/{id:int}", UpdateModifierExtra);

            group.MapDelete("/{id:int}", DeleteModifierExtra);

            group.MapGet("/getbyname/{name}", GetModifierExtrasByName);            

            return group;
        }

        static async Task<Ok<List<ModifierExtraDTO>>> GetModifierExtras(IRepositoryModifierExtras repository, IMapper mapper,
            int page = 1, int recordsByPage = 10)
        {
            var pagination = new PaginationDTO { Page = page, RecordsByPage = recordsByPage };
            var modifierExtras = await repository.GetAll(pagination);
            var modifierExtrasDTO = mapper.Map<List<ModifierExtraDTO>>(modifierExtras); //modifierExtras.Select(x => new  ModifierExtraDTO { Id = x.Id, Name = x.Name}).ToList();
            return TypedResults.Ok(modifierExtrasDTO);
        }

        static async Task<Ok<List<ModifierExtraDTO>>> GetModifierExtrasByName(string name, IRepositoryModifierExtras repository, IMapper mapper)
        {

            var modifierExtras = await repository.GetByName(name);
            var modifierExtrasDTO = mapper.Map<List<ModifierExtraDTO>>(modifierExtras);
            return TypedResults.Ok(modifierExtrasDTO);
        }

        static async Task<Results<Ok<ModifierExtraDTO>, NotFound>> GetModifierExtrasById(IRepositoryModifierExtras repository, int id
            , IMapper mapper)
        {
            var modifierExtra = await repository.GetById(id);

            if (modifierExtra == null)
            {
                return TypedResults.NotFound();
            }

            var modifierExtraDTO = mapper.Map<ModifierExtraDTO>(modifierExtra);

            return TypedResults.Ok(modifierExtraDTO);
        }

        static async Task<Created<ModifierExtraDTO>> CreateModifierExtra(CreateModifierExtraDTO createModifierExtraDTO, IRepositoryModifierExtras repository
            , IMapper mapper)
        {
            var modifierExtra = mapper.Map<ModifierExtra> (createModifierExtraDTO);

            var id = await repository.Create(modifierExtra);

            var modifierExtraDTO = mapper.Map<ModifierExtraDTO>(modifierExtra);

            return TypedResults.Created($"/modifierExtras/{id}", modifierExtraDTO);
        }

        static async Task<Results<NoContent, NotFound>> UpdateModifierExtra(int id, CreateModifierExtraDTO createModifierExtraDTO, IRepositoryModifierExtras repository
            , IMapper mapper)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            var modifierExtra = mapper.Map<ModifierExtra>(createModifierExtraDTO);
            modifierExtra.Id = id;

            await repository.Update(modifierExtra);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> DeleteModifierExtra(int id, IRepositoryModifierExtras repository)
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
