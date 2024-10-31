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
            group.MapPost("/", Create).DisableAntiforgery();
            return group;
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
