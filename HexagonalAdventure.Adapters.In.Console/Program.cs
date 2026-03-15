using HexagonalAdventure.Adapters.Out.InMemory;
using HexagonalAdventure.Application.Ports.Inbound;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Application.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IProductRepository, InMemoryProdutRepository>()
    .AddScoped<IProductService, ProductService>()
    .BuildServiceProvider();

Console.WriteLine("Add a new product");

Console.Write("Title: ");
string title = Console.ReadLine();

Console.Write("Price: ");
decimal price = decimal.Parse(Console.ReadLine());

Console.Write("Product Code: ");
string productCode = Console.ReadLine();

Console.Write("Stock: ");
int stock = int.Parse(Console.ReadLine());

var productService = serviceProvider.GetRequiredService<IProductService>();
var newProductId = productService.CreateProduct(title, productCode, price, Guid.NewGuid(), stock);

Console.WriteLine("Product created with ID: " + newProductId);
Console.ReadLine();
