using System;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Product_service.Helper;
using Product_service.Persistence;
using Product_service.Repository;
using Product_service.Service;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName; // "Development", "Production", etc.

var keyVaultUri = environment == "Development"
    ? new Uri("https://keyvault-eccomerce-devv3.vault.azure.net/")
    : new Uri("https://keyvault-eccomerce-prodv3.vault.azure.net/");

builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

// Get Cosmos settings from configuration
var cosmosConnectionString = builder.Configuration["CosmosDbConnectionString"];
var cosmosDbName = builder.Configuration["CosmosDbDatabaseName"];

// Configure Cosmos DB
builder.Services.AddDbContext<CosmoDbContext>(options =>
    options.UseCosmos(cosmosConnectionString, cosmosDbName));

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
