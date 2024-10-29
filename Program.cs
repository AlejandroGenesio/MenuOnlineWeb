using MenuOnlineUdemy;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services START

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositoryProducts, RepositoryProducts>();
builder.Services.AddScoped<IRepositoryImages, RepositoryImages>();

// Services END

var app = builder.Build();

// Middleware START

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

var enpointsProducts = app.MapGroup("/products");

enpointsProducts.MapGet("/", GetProducts);

enpointsProducts.MapGet("/{id:int}", GetProductsById);

enpointsProducts.MapPost("/", CreateProduct);

enpointsProducts.MapPut("/{id:int}", UpdateProduct);

enpointsProducts.MapDelete("/{id:int}", DeleteProduct);

// Middleware END

app.Run();

static async Task<Ok<List<Product>>> GetProducts (IRepositoryProducts repository)
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