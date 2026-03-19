using HexagonalAdventure.Adapters.Out.Email;
using HexagonalAdventure.Adapters.Out.InMemory;
using HexagonalAdventure.Application.Events;
using HexagonalAdventure.Application.Ports.Inbound;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Application.Services;
using HexagonalAdventure.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceProvider = new ServiceCollection()
    .AddLogging(configure => {
        configure.AddConsole();
        configure.SetMinimumLevel(LogLevel.Debug);
    })
    .AddSingleton<IProductRepository, InMemoryProdutRepository>()
    .AddTransient<IEventDispatcher, EventDispatcher>()
    .AddTransient<IEmailService, SmtpEmailAdapter>()
    .AddTransient<IDomainEventHandler<ProductCreatedEvent>, ProductCreatedEmailNotificationHandler>()
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
var newProductId = await productService.CreateProduct(title, productCode, price, Guid.NewGuid(), stock);

Console.WriteLine("Product created with ID: " + newProductId);
Console.ReadLine();
