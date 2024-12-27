using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MenuOnlineUdemy.Endpoints
{
    public static class ModifierGroupEndpoints
    {
        public static RouteGroupBuilder MapModifierGroups(this RouteGroupBuilder group)
        {

            group.MapGet("/", GetModifierGroups);

            group.MapGet("/{id:int}", GetModifierGroupsById);

            group.MapPost("/", CreateModifierGroup);

            group.MapPut("/{id:int}", UpdateModifierGroup);

            group.MapDelete("/{id:int}", DeleteModifierGroup);

            group.MapGet("/getbyname/{name}", GetModifierGroupsByName);

            group.MapPost("/{id:int}/assignmodifieroptions", AssignModifierOption);

            return group;
        }

        static async Task<Ok<List<ModifierGroupDTO>>> GetModifierGroups(IRepositoryModifierGroups repository, IMapper mapper,
            int page = 1, int recordsByPage = 10)
        {
            var pagination = new PaginationDTO { Page = page, RecordsByPage = recordsByPage };
            var modifierGroups = await repository.GetAll(pagination);
            var modifierGroupsDTO = mapper.Map<List<ModifierGroupDTO>>(modifierGroups); //modifierGroups.Select(x => new  ModifierGroupDTO { Id = x.Id, Name = x.Name}).ToList();
            return TypedResults.Ok(modifierGroupsDTO);
        }

        static async Task<Ok<List<ModifierGroupDTO>>> GetModifierGroupsByName(string name, IRepositoryModifierGroups repository, IMapper mapper)
        {

            var modifierGroups = await repository.GetByName(name);
            var modifierGroupsDTO = mapper.Map<List<ModifierGroupDTO>>(modifierGroups);
            return TypedResults.Ok(modifierGroupsDTO);
        }

        static async Task<Results<Ok<ModifierGroupDTO>, NotFound>> GetModifierGroupsById(IRepositoryModifierGroups repository, int id
            , IMapper mapper)
        {
            var modifierGroup = await repository.GetById(id);

            if (modifierGroup == null)
            {
                return TypedResults.NotFound();
            }

            var modifierGroupDTO = mapper.Map<ModifierGroupDTO>(modifierGroup);

            return TypedResults.Ok(modifierGroupDTO);
        }

        static async Task<Created<ModifierGroupDTO>> CreateModifierGroup(CreateModifierGroupDTO createModifierGroupDTO, IRepositoryModifierGroups repository
            , IMapper mapper)
        {
            var modifierGroup = mapper.Map<ModifierGroup> (createModifierGroupDTO);

            var id = await repository.Create(modifierGroup);

            var modifierGroupDTO = mapper.Map<ModifierGroupDTO>(modifierGroup);

            return TypedResults.Created($"/modifiergroups/{id}", modifierGroupDTO);
        }

        static async Task<Results<NoContent, NotFound>> UpdateModifierGroup(int id, CreateModifierGroupDTO createModifierGroupDTO, IRepositoryModifierGroups repository
            , IMapper mapper)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            var modifierGroup = mapper.Map<ModifierGroup>(createModifierGroupDTO);
            modifierGroup.Id = id;

            await repository.Update(modifierGroup);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> DeleteModifierGroup(int id, IRepositoryModifierGroups repository)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent, BadRequest<string>>> AssignModifierOption(int id,
            List<int> modifierOptionsIds,
            IRepositoryModifierGroups repositoryModifierGroups, IRepositoryModifierOptions repositoryModifierOptions)
        {
            if (!await repositoryModifierGroups.IfExists(id))
            {
                return TypedResults.NotFound();
            }

            var modifierOptionsExisting = new List<int>();
            if (modifierOptionsIds.Count != 0)
            {
                modifierOptionsExisting = await repositoryModifierOptions.IfTheyExists(modifierOptionsIds);
            }

            if (modifierOptionsExisting.Count != modifierOptionsIds.Count)
            {
                var modifierOptionsNonExisting = modifierOptionsIds.Except(modifierOptionsExisting);

                return TypedResults.BadRequest($"Modifier Options id {string.Join(",", modifierOptionsNonExisting)} do not exist.");
            }

            await repositoryModifierGroups.AssignModifierOption(id, modifierOptionsIds);
            return TypedResults.NoContent();
        }
    }
}
