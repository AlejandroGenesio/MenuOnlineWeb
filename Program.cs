using MenuOnlineUdemy;
using MenuOnlineUdemy.Endpoints;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using MenuOnlineUdemy.services;
using MenuOnlineUdemy.services.Import;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services START

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositoryProducts, RepositoryProducts>();
builder.Services.AddScoped<IRepositoryVariants, RepositoryVariants>();
builder.Services.AddScoped<IRepositoryModifierExtras, RepositoryModifierExtras>();
builder.Services.AddScoped<IRepositoryModifierGroups, RepositoryModifierGroups>();
builder.Services.AddScoped<IRepositoryImages, RepositoryImages>();
builder.Services.AddScoped<IRepositoryOrders, RepositoryOrders>();

builder.Services.AddScoped<IFileStorage, LocalStorage>();
//builder.Services.AddSingleton<IProductBulkImportHandler, ProductBulkImportHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));




// Services END

var app = builder.Build();

// Middleware START

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.MapGroup("/products").MapProducts();
app.MapGroup("/product/{productId:int}/variants").MapVariants();
app.MapGroup("/modifierextras").MapModifierExtras();
app.MapGroup("/modifiergroups").MapModifierGroups();
app.MapGroup("/images").MapImages();
app.MapGroup("/orders").MapOrders();

// Middleware END

app.Run();