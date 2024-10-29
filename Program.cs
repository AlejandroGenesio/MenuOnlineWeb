using MenuOnlineUdemy;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
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

enpointsProducts.MapGet("/", async (IRepositoryProducts repository) =>
{

    return await repository.GetAll();
});

enpointsProducts.MapGet("/{id:int}", async (IRepositoryProducts repository, int id) =>
{
    var product = await repository.GetById(id);

    if (product == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(product);
});

enpointsProducts.MapPost("/", async (Product product, IRepositoryProducts repository) =>
{
    var id = await repository.Create(product);
    return Results.Created($"/products/{id}", product);
});

enpointsProducts.MapPut("/{id:int}", async (int id, Product product, IRepositoryProducts repository) =>
{
    var exists = await repository.IfExists(id);
    if (!exists)
    {
        return Results.NotFound();
    }

    await repository.Update(product);
    return Results.NoContent();
});

enpointsProducts.MapDelete("/{id:int}", async (int id, IRepositoryProducts repository) =>
{
    var exists = await repository.IfExists(id);
    if (!exists)
    {
        return Results.NotFound();
    }

    await repository.Delete(id);
    return Results.NoContent();
});

// Middleware END

app.Run();
