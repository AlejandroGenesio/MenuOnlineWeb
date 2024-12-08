using FluentValidation;
using MenuOnlineUdemy;
using MenuOnlineUdemy.Endpoints;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using MenuOnlineUdemy.services;
using MenuOnlineUdemy.services.Import;
using MenuOnlineUdemy.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Services START

// Authentication
builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));
builder.Services.AddHttpContextAccessor();

// Frontend
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Backend
builder.Services.AddScoped<IRepositoryProducts, RepositoryProducts>();
builder.Services.AddScoped<IRepositoryVariants, RepositoryVariants>();
builder.Services.AddScoped<IRepositoryModifierOptions, RepositoryModifierOptions>();
builder.Services.AddScoped<IRepositoryModifierGroups, RepositoryModifierGroups>();
builder.Services.AddScoped<IRepositoryImages, RepositoryImages>();
builder.Services.AddScoped<IRepositoryOrders, RepositoryOrders>();
builder.Services.AddScoped<IRepositoryCategories, RepositoryCategories>();
builder.Services.AddScoped<IRepositoryOrderDetails, RepositoryOrderDetails>();

builder.Services.AddScoped<IFileStorage, LocalStorage>();
builder.Services.AddScoped<IProductBulkImportHandler, ProductBulkImportHandler>();
builder.Services.AddScoped<ProductBulkImportBusinessLogic>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));

// Validation rules
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

EnableExcelLicense();

// Authentication
builder.Services.AddAuthentication().AddJwtBearer(options =>
options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    //IssuerSigningKey = AuthKeys.GetKey(builder.Configuration).First(),
    IssuerSigningKeys = AuthKeys.GetAllKey(builder.Configuration),
    ClockSkew = TimeSpan.Zero
});
builder.Services.AddAuthorization();


// Services END

var app = builder.Build();

// Middleware START

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

// Authentication
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.MapGroup("/products").MapProducts();
app.MapGroup("/product/{productId:int}/variants").MapVariants();
app.MapGroup("/modifierOptions").MapModifierOptions();
app.MapGroup("/modifiergroups").MapModifierGroups();
app.MapGroup("/images").MapImages();
app.MapGroup("/orders").MapOrders();
app.MapGroup("/order/{orderId:int}/orderdetails").MapOrderDetails();
app.MapGroup("/categories").MapCategories();
app.MapGroup("/users").MapUsers();

// Middleware END

app.Run();

static void EnableExcelLicense()
{
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
}