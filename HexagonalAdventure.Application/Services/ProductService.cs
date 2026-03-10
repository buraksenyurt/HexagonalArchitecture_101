using HexagonalAdventure.Application.Ports.Inbound;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Application.Services;

public class ProductService(IProductRepository productRepository)
    : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;

    public Guid CreateProduct(string title, decimal price, string category, int stock)
    {
        // Domain nesnesi oluşturulur ve orada tanımlı iş kuralları da yürütülür.
        var product = new Product(Guid.NewGuid(), title, price, category, stock);
        // Outbound port olarak tanımladığımız arayüz üzerinden ürün ekleme işlevi çağırılır
        _productRepository.AddProduct(product);
        return product.Id;
    }
}
