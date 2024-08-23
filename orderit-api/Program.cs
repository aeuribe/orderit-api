
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using orderit_api.Data;
using orderit_api.Interfaces;
using orderit_api.Repository;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// Configurar la conexi�n a la base de datos
var BusinessConnectionString = builder.Configuration.GetConnectionString("BusinessPostgreSQLConnection");
builder.Services.AddDbContext<BusinessContext>(options =>
    options.UseNpgsql(BusinessConnectionString));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro de repositorios
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<ISalespersonRepository, SalespersonRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Registro de Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


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
