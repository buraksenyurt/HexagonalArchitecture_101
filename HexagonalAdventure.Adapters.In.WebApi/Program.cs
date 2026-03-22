using HexagonalAdventure.Adapters.Out.EF;
using HexagonalAdventure.Adapters.Out.Email;

// using HexagonalAdventure.Adapters.Out.InMemory;
using HexagonalAdventure.Application.Events;
using HexagonalAdventure.Application.Ports.Inbound;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Application.Services;
using HexagonalAdventure.Domain.Events;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();

// EF Postgresql kullanımı için de middleware'e bir şeyler eklememiz lazım
builder.Services.AddDbContext<DeppoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DeppoConnectionString")));

// Dependency Injection tanımlamaları
// builder.Services.AddSingleton<IProductRepository, InMemoryProdutRepository>(); // Tüm uygulama boyunca tek bir instance kullanılır

// Gerekli bileşene bağımlılıklarını DI servislerine ekliyoruz
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddTransient<IEmailService, SmtpEmailAdapter>();
builder.Services.AddTransient<IDomainEventHandler<ProductCreatedEvent>, ProductCreatedEmailNotificationHandler>();
builder.Services.AddSingleton<IConnectionMultiplexer>(
    sp => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnectionString"))
   );

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
