using HexagonalAdventure.Adapters.Out.InMemory;
using HexagonalAdventure.Application.Ports.Inbound;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Application.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Dependency Injection tanımlamaları
builder.Services.AddSingleton<IProductRepository, InMemoryProdutRepository>(); // Tüm uygulama boyunca tek bir instance kullanılır
builder.Services.AddScoped<IProductService, ProductService>();

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
