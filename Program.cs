using MenuOnlineUdemy;
using MenuOnlineUdemy.Endpoints;
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

builder.Services.AddAutoMapper(typeof(Program));

// Services END

var app = builder.Build();

// Middleware START

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.MapGroup("/products").MapProducts();
app.MapGroup("/images").MapImages();

// Middleware END

app.Run();