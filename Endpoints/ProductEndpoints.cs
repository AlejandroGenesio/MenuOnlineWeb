using AutoMapper;
using FluentValidation;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using MenuOnlineUdemy.services.Import;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MenuOnlineUdemy.Endpoints
{
    public static class ProductEndpoints
    {
        public static RouteGroupBuilder MapProducts(this RouteGroupBuilder group)
        {

            group.MapGet("/", GetProducts);

            group.MapGet("/{id:int}", GetProductsById);

            group.MapPost("/", CreateProduct).RequireAuthorization();

            group.MapPut("/{id:int}", UpdateProduct).RequireAuthorization();

            group.MapDelete("/{id:int}", DeleteProduct).RequireAuthorization();

            group.MapGet("/getbyname/{name}", GetProductsByName);

            group.MapPost("/{id:int}/assignimages", AssignImages).RequireAuthorization();

            group.MapPost("/{id:int}/assignmodifiergroups", AssignModifierGroup).RequireAuthorization();

            group.MapPost("/{id:int}/assigncategories", AssignCategories).RequireAuthorization();

            group.MapPost("/bulkProductImport", ImportProductFile).DisableAntiforgery().RequireAuthorization();

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

        static async Task<Results<Created<ProductDTO>, ValidationProblem>>
            CreateProduct(CreateProductDTO createProductDTO, IRepositoryProducts repository
            , IMapper mapper, IValidator<CreateProductDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(createProductDTO);
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var product = mapper.Map<Product>(createProductDTO);

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

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AssignImages(int id, List<int> imagesIds,
            IRepositoryProducts repositoryProducts, IRepositoryImages repositoryImages)
        {
            if (!await repositoryProducts.IfExists(id))
            {
                return TypedResults.NotFound();
            }

            var imagesExisting = new List<int>();
            if (imagesIds.Count != 0)
            {
                imagesExisting = await repositoryImages.IfTheyExist(imagesIds);
            }

            if (imagesExisting.Count != imagesIds.Count)
            {
                var imagesNonExisting = imagesIds.Except(imagesExisting);

                return TypedResults.BadRequest($"Images id {string.Join(",", imagesNonExisting)} do not exist.");
            }

            await repositoryProducts.AssignImages(id, imagesIds);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AssignCategories(int id, List<int> categoriesIds,
            IRepositoryProducts repositoryProducts, IRepositoryCategories repositoryCategories)
        {
            if (!await repositoryProducts.IfExists(id))
            {
                return TypedResults.NotFound();
            }

            var categoriesExisting = new List<int>();
            if (categoriesIds.Count != 0)
            {
                categoriesExisting = await repositoryCategories.IfTheyExist(categoriesIds);
            }

            if (categoriesExisting.Count != categoriesIds.Count)
            {
                var categoriesNonExisting = categoriesIds.Except(categoriesExisting);

                return TypedResults.BadRequest($"Categories id {string.Join(",", categoriesNonExisting)} do not exist.");
            }

            await repositoryProducts.AssignCategories(id, categoriesIds);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent, BadRequest<string>>> AssignModifierGroup(int id,
            List<int> modifierGroupsIds,
            IRepositoryProducts repositoryProducts, IRepositoryModifierGroups repositoryModifierGroups)
        {
            if (!await repositoryProducts.IfExists(id))
            {
                return TypedResults.NotFound();
            }

            var modifierGroupsExisting = new List<int>();
            if (modifierGroupsIds.Count != 0)
            {
                modifierGroupsExisting = await repositoryModifierGroups.IfTheyExist(modifierGroupsIds);
            }

            if (modifierGroupsExisting.Count != modifierGroupsIds.Count)
            {
                var modifierGroupsNonExisting = modifierGroupsIds.Except(modifierGroupsExisting);

                return TypedResults.BadRequest($"Modifier Groups id {string.Join(",", modifierGroupsNonExisting)} do not exist.");
            }

            await repositoryProducts.AssignModifierGroup(id, modifierGroupsIds);
            return TypedResults.NoContent();
        }

        static async Task<IActionResult> ImportProductFile(IFormFile file, IProductBulkImportHandler productBulkImportHandler)
        {
            bool importSucceeded = await productBulkImportHandler.Import(file);

            return new OkObjectResult($"OK = {importSucceeded}");
        }

    }
}
