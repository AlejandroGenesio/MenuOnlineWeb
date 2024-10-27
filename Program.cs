using MenuOnlineUdemy.Entities;

var builder = WebApplication.CreateBuilder(args);

// Services START

builder.Services.AddSwaggerGen();

// Services END

var app = builder.Build();

// Middleware START

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/products", () =>
{
    var products = new List<Product> {
        new Product
        {
            Id = 1,
            Name = "test1"
        },

        new Product {
            Id = 2,
            Name = "test2"
        }};

    return products;
});
app.MapGet("/", () => "Hello World!");

// Middleware END

app.Run();
