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
using Azure.Extensions.AspNetCore.Configuration.Secrets;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole(); 


var environment = Environment.GetEnvironmentVariable("ENVIRONMENT_NAME");
Console.WriteLine($"ENV: {environment}");

var keyVaultUriEnv = Environment.GetEnvironmentVariable("KEYVAULT_URI");
Console.WriteLine($"KEYVAULT_URI = {keyVaultUriEnv}");



var productCosmosConnectionString = Environment.GetEnvironmentVariable("COSMOS_DB_URI");
var productCosmosDbName = Environment.GetEnvironmentVariable("COSMOS_PRODUCTDB_NAME");

var categoryCosmosConnectionString = Environment.GetEnvironmentVariable("COSMOS_DB_URI");
var categoryCosmosDbName = Environment.GetEnvironmentVariable("COSMOS_INVENTORYDB_NAME");

builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseCosmos(productCosmosConnectionString!, productCosmosDbName!);
});

builder.Services.AddDbContext<CategoryDbContext>(options =>
{
    options.UseCosmos(categoryCosmosConnectionString!, categoryCosmosDbName!);
});

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
