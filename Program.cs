using MenuOnlineUdemy;
using MenuOnlineUdemy.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services START

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services END

var app = builder.Build();

// Middleware START

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

// Middleware END

app.Run();
