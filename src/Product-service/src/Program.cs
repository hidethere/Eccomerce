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
builder.Logging.AddConsole();

var config = builder.Configuration;


string cosmosUri;
string cosmosDbNameProduct;
string cosmosDbNameCategory;


if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("Running in Development Environment");
    cosmosUri = config["CosmosDb:UriLocal"]!;
    cosmosDbNameProduct = config["CosmosDb:ProductDbName"]!;
    cosmosDbNameCategory = config["CosmosDb:CategoryDbName"]!;
}
else
{
    cosmosUri = Environment.GetEnvironmentVariable("COSMOS_DB_URI");
    cosmosDbNameProduct = Environment.GetEnvironmentVariable("COSMOS_PRODUCTDB_NAME");
    cosmosDbNameCategory = Environment.GetEnvironmentVariable("COSMOS_CATEGORYDB_NAME");

}

// Product DB context setup
builder.Services.AddDbContext<ProductDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Use connection string for local dev
        options.UseCosmos(
            connectionString: cosmosUri!,
            databaseName: cosmosDbNameProduct!
        );
    }
    else
    {
        // Use token credential in production
        var credential = new DefaultAzureCredential();
        options.UseCosmos(
            accountEndpoint: cosmosUri!,
            tokenCredential: credential,
            databaseName: cosmosDbNameProduct!
        );
    }
});

// Category DB context setup
builder.Services.AddDbContext<CategoryDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseCosmos(
            connectionString: cosmosUri!,
            databaseName: cosmosDbNameCategory!
        );
    }
    else
    {
        var credential = new DefaultAzureCredential();
        options.UseCosmos(
            accountEndpoint: cosmosUri!,
            tokenCredential: credential,
            databaseName: cosmosDbNameCategory!
        );
    }
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
