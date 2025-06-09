using System;
using System.Reflection.Emit;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Product_service.Domain;
using Product_service.Helper;
using Product_service.Persistence;
using Product_service.Repository;
using Product_service.Service;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName; // "Development", "Production", etc.


var keyVaultUriEnv = Environment.GetEnvironmentVariable("KEYVAULT_URI");

if (string.IsNullOrEmpty(keyVaultUriEnv))
{
    throw new InvalidOperationException("Environment variable 'KEYVAULT_URI' is not set.");
}

var keyVaultUri = new Uri(keyVaultUriEnv);
builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

var productCosmosConnectionString = builder.Configuration["CosmosDbConnectionString"];
var productCosmosDbName = builder.Configuration["ProductCosmosDbDatabaseName"];

var categoryCosmosConnectionString = builder.Configuration["CosmosDbConnectionString"];
var categoryCosmosDbName = builder.Configuration["CategoryCosmosDbDatabaseName"];

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseCosmos(productCosmosConnectionString, productCosmosDbName));

builder.Services.AddDbContext<CategoryDbContext>(options =>
    options.UseCosmos(categoryCosmosConnectionString, categoryCosmosDbName));

builder.Services.AddScoped<IProductMapper, ProductMapper>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
