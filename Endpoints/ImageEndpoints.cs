using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using MenuOnlineUdemy.services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MenuOnlineUdemy.Endpoints
{
    public static class ImageEndpoints
    {
        private static readonly string container = "images";
        public static RouteGroupBuilder MapImages(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll);
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/", Create).DisableAntiforgery();
            group.MapPut("/{id:int}", Update).DisableAntiforgery();
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<ImageDTO>>> GetAll(IRepositoryImages repository, IMapper mapper)
        {
            var images = await repository.GetAll();
            var imagesDTO = mapper.Map<List<ImageDTO>>(images);
            return TypedResults.Ok(imagesDTO);
        }

        static async Task<Results<Ok<ImageDTO>, NotFound>> GetById(int id, IRepositoryImages repository, IMapper mapper)
        {
            var image = await repository.GetById(id);
            if(image == null)
            {
                return TypedResults.NotFound();
            }

            var imageDTO = mapper.Map<ImageDTO>(image);

            return TypedResults.Ok(imageDTO);
        }

        static async Task<Created<ImageDTO>> Create([FromForm] CreateImageDTO createImageDTO,
            IRepositoryImages repository, IMapper mapper,
            IFileStorage fileStorage)
        {
            var image = mapper.Map<Image>(createImageDTO);

            if (createImageDTO.File != null)
            {
                var url = await fileStorage.Storage(container, createImageDTO.File);
                image.File = url;
            }

            var id = await repository.Create(image);

            var imageDTO = mapper.Map<ImageDTO>(image);

            return TypedResults.Created($"/images/{id}", imageDTO);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id, [FromForm] CreateImageDTO createImageDTO, IRepositoryImages repository,
            IFileStorage fileStorage, IMapper mapper)
        {
            var imageDB = await repository.GetById(id);
            if (imageDB == null)
            {
                return TypedResults.NotFound();
            }

            var imageToUpdate = mapper.Map<Image>(createImageDTO);
            imageToUpdate.Id = id;
            imageToUpdate.File = imageDB.File;

            if(createImageDTO.File is not null)
            {
                var url = await fileStorage.Edit(imageToUpdate.File,
                    container, createImageDTO.File);

                imageToUpdate.File = url;
            }
            await repository.Update(imageToUpdate);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepositoryImages repository,
            IFileStorage fileStorage)
        {
            var imageDB = await repository.GetById(id);
            if(imageDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            await fileStorage.Delete(imageDB.File, container);
            return TypedResults.NoContent();
        }

    }
}
