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

    }
}
