using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MenuOnlineUdemy.Endpoints
{
    public static class ModifierOptionEndpoints
    {
        public static RouteGroupBuilder MapModifierOptions(this RouteGroupBuilder group)
        {

            group.MapGet("/", GetModifierOptions);

            group.MapGet("/{id:int}", GetModifierOptionsById);

            group.MapPost("/", CreateModifierOption);

            group.MapPut("/{id:int}", UpdateModifierOption);

            group.MapDelete("/{id:int}", DeleteModifierOption);

            group.MapGet("/getbyname/{name}", GetModifierOptionsByName);            

            return group;
        }

        static async Task<Ok<List<ModifierOptionDTO>>> GetModifierOptions(IRepositoryModifierOptions repository, IMapper mapper,
            int page = 1, int recordsByPage = 10)
        {
            var pagination = new PaginationDTO { Page = page, RecordsByPage = recordsByPage };
            var modifierOptions = await repository.GetAll(pagination);
            var modifierOptionsDTO = mapper.Map<List<ModifierOptionDTO>>(modifierOptions); //modifierOptions.Select(x => new  ModifierOptionDTO { Id = x.Id, Name = x.Name}).ToList();
            return TypedResults.Ok(modifierOptionsDTO);
        }

        static async Task<Ok<List<ModifierOptionDTO>>> GetModifierOptionsByName(string name, IRepositoryModifierOptions repository, IMapper mapper)
        {

            var modifierOptions = await repository.GetByName(name);
            var modifierOptionsDTO = mapper.Map<List<ModifierOptionDTO>>(modifierOptions);
            return TypedResults.Ok(modifierOptionsDTO);
        }

        static async Task<Results<Ok<ModifierOptionDTO>, NotFound>> GetModifierOptionsById(IRepositoryModifierOptions repository, int id
            , IMapper mapper)
        {
            var modifierOption = await repository.GetById(id);

            if (modifierOption == null)
            {
                return TypedResults.NotFound();
            }

            var modifierOptionDTO = mapper.Map<ModifierOptionDTO>(modifierOption);

            return TypedResults.Ok(modifierOptionDTO);
        }

        static async Task<Created<ModifierOptionDTO>> CreateModifierOption(CreateModifierOptionDTO createModifierOptionDTO, IRepositoryModifierOptions repository
            , IMapper mapper)
        {
            var modifierOption = mapper.Map<ModifierOption> (createModifierOptionDTO);

            var id = await repository.Create(modifierOption);

            var modifierOptionDTO = mapper.Map<ModifierOptionDTO>(modifierOption);

            return TypedResults.Created($"/modifierOptions/{id}", modifierOptionDTO);
        }

        static async Task<Results<NoContent, NotFound>> UpdateModifierOption(int id, CreateModifierOptionDTO createModifierOptionDTO, IRepositoryModifierOptions repository
            , IMapper mapper)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            var modifierOption = mapper.Map<ModifierOption>(createModifierOptionDTO);
            modifierOption.Id = id;

            await repository.Update(modifierOption);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> DeleteModifierOption(int id, IRepositoryModifierOptions repository)
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
