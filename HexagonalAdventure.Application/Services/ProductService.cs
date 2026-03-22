using HexagonalAdventure.Application.Events;
using HexagonalAdventure.Application.Ports.Inbound;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Application.Services;

public class ProductService(IProductRepository productRepository, IEventDispatcher eventDispatcher)
    : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;

    /*
        CreateProduct metodu bir ürün oluşturmak için gerekli bilgileri alır.
        Product bir entity ve ProductCode bir value object'tir. 
        Primitive tipler alınmasının sebebi Web Api Controller veya Console uygulamasının 
        ProductCode gibi bir value object'i tanımaması ve doğrudan string gibi primitive bir tiple çalışmasıdır.
        Zira Inbound Adapter'ların Domain katmanındaki nesneleri tanımaması ve doğrudan primitive tiplerle çalışması beklenir.
    */
    public async Task<Guid> CreateProduct(string title, string productCode, decimal price, Guid categoryId, int stock)
    {
        var code = new ProductCode(productCode); // ProductCode içindeki iş luralları da çalışır ve bir hata varsa kirli daha oluşmaz zira Repository çalıştırılmaz.
        // Domain nesnesi oluşturulur ve orada tanımlı iş kuralları da yürütülür.
        var product = new Product(Guid.NewGuid(), code, title, price, categoryId, stock);
        // Outbound port olarak tanımladığımız arayüz üzerinden ürün ekleme işlevi çağırılır
        await _productRepository.AddProductAsync(product);

        var domainEvents = product.DomainEvents.ToList(); 
        // iterasyon sırasında DomainEvents koleksiyonunda değişiklik olmaması için bir kopyaasını alıyoruz.
        product.ClearDomainEvents(); // product nesnesi üzerindeki event listesini temizliyoruz,
        // çünkü eventler tetiklendikten sonra tekrar tetiklenmemesi gerekir.
        
        // Burada da olayları dolaşıp tetikliyoruz.
        foreach (var domainEvent in domainEvents)
        {
            await _eventDispatcher.DispatchAsync(domainEvent);
        }

        return product.Id;
    }

    public async Task IncreaseProductStock(Guid productId, int value)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) throw new KeyNotFoundException("Product not found with the specified ID");
        product.IncreaseStock(value);
        await _productRepository.UpdateProductAsync(product);

        var domainEvents = product.DomainEvents.ToList();
        product.ClearDomainEvents();
        foreach(var domainEvent in domainEvents)
        {
            await _eventDispatcher.DispatchAsync(domainEvent);
        }
    }

    public async Task<Product> GetProductById(Guid productId) {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) throw new KeyNotFoundException("Product not found with the specified ID");
        return product;
    }
}
