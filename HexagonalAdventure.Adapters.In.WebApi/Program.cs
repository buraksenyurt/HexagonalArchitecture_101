using HexagonalAdventure.Adapters.Out.EF;
// using HexagonalAdventure.Adapters.Out.InMemory;
using HexagonalAdventure.Application.Ports.Inbound;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// EF Postgresql kullanımı için de middleware'e bir şeyler eklememiz lazım
builder.Services.AddDbContext<DeppoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DeppoConnectionString")));

// Dependency Injection tanımlamaları
// builder.Services.AddSingleton<IProductRepository, InMemoryProdutRepository>(); // Tüm uygulama boyunca tek bir instance kullanılır

// EF Core kullanan yeni outbound port implementasyonu
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
