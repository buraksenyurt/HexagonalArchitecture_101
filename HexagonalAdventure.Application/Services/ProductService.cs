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
        _productRepository.AddProduct(product);

        // Kayıtlı domain olaylarını tetikleyelim
        foreach(var domainEvent in product.DomainEvents)
        {
            // DispatcAsync asenkron bir metot olduğundan CreateProduct'ın dönüşü de değiştirilmelidir.
            // IProductService'de buna göre değiştirilmiştir.
            await _eventDispatcher.DispatchAsync(domainEvent);
        }
        product.ClearDomainEvents(); // Olaylar tetiklendikten sonra Entity nesnesine kayıtlı olanları temizliyoruz.

        return product.Id;
    }
}
