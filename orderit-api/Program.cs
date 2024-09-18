
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using orderit_api.Data;
using orderit_api.Interfaces;
using orderit_api.Repository;
using orderit_api.Seeders;
using orderit_api.Services;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);


// Configurar la conexi�n a la base de datos
var BusinessConnectionString = Environment.GetEnvironmentVariable("DB_BUSINESS_CONNECTION_STRING") ?? 
    builder.Configuration.GetConnectionString("BusinessPostgreSQLConnection");

builder.Services.AddDbContext<BusinessContext>(options =>
    options.UseNpgsql(BusinessConnectionString));

// Configurar la conexi�n a la base de datos Identity
var IdentityConnectionString = Environment.GetEnvironmentVariable("DB_IDENTITY_CONNECTION_STRING") ??
    builder.Configuration.GetConnectionString("IdentityPostgreSQLConnection");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseNpgsql(IdentityConnectionString));


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

//Registro de los seeders
builder.Services.AddTransient<RoleSeeder>();

// Registro de Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Registro de Identity
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
    
    }).AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

var key = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(key))
{
    throw new InvalidOperationException("JWT Key configuration is missing.");
}


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    Console.WriteLine($"JWT Key: {builder.Configuration.GetSection("Jwt:Key").Value}");
    Console.WriteLine($"JWT Key: {builder.Configuration.GetSection("Jwt:Issuer").Value}");
    Console.WriteLine($"JWT Key: {builder.Configuration.GetSection("Jwt:Audience").Value}");
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
    };
        
});

//Agregando la configuracion de CORS
builder.Services.AddCors(options => {

    //ReactApp configuration
    options.AddPolicy("reactApp", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:5173");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Calling the seeders
using (var scope = app.Services.CreateScope())
{
    var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
    await roleSeeder.SeedRolesAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //autenticacion siempre va antes que Authorization

app.UseAuthorization();

app.MapControllers();

app.UseCors("reactApp");

app.Run();
