using HexagonalAdventure.Application.Ports.Inbound;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Application.Services;

public class ProductService(IProductRepository productRepository)
    : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;

    /*
        CreateProduct metodu bir ürün oluşturmak için gerekli bilgileri alır.
        Product bir entity ve ProductCode bir value object'tir. 
        Primitive tipler alınmasının sebebi Web Api Controller veya Console uygulamasının 
        ProductCode gibi bir value object'i tanımaması ve doğrudan string gibi primitive bir tiple çalışmasıdır.
        Zira Inbound Adapter'ların Domain katmanındaki nesneleri tanımaması ve doğrudan primitive tiplerle çalışması beklenir.
    */
    public Guid CreateProduct(string title, string productCode, decimal price, Guid categoryId, int stock)
    {
        var code = new ProductCode(productCode); // ProductCode içindeki iş luralları da çalışır ve bir hata varsa kirli daha oluşmaz zira Repository çalıştırılmaz.
        // Domain nesnesi oluşturulur ve orada tanımlı iş kuralları da yürütülür.
        var product = new Product(Guid.NewGuid(), code, title, price, categoryId, stock);
        // Outbound port olarak tanımladığımız arayüz üzerinden ürün ekleme işlevi çağırılır
        _productRepository.AddProduct(product);
        return product.Id;
    }
}
