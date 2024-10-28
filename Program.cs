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

app.MapGet("/products", async (IRepositoryProducts repository) =>
{

    return await repository.GetAll();
});

app.MapGet("/products/{id:int}", async (IRepositoryProducts repository, int id) =>
{
    var product = await repository.GetById(id);

    if (product == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(product);
});

app.MapPost("/products", async (Product product, IRepositoryProducts repository) =>
{
    var id = await repository.Create(product);
    return Results.Created($"/products/{id}", product);
});

app.MapPut("/products/{id:int}", async (int id, Product product, IRepositoryProducts repository) =>
{
    var exists = await repository.IfExists(id);
    if (!exists)
    {
        return Results.NotFound();
    }

    await repository.Update(product);
    return Results.NoContent();
});

app.MapDelete("/products/{id:int}", async (int id, IRepositoryProducts repository) =>
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
