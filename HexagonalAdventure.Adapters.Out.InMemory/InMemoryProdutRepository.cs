using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Adapters.Out.InMemory;

public class InMemoryProdutRepository
    : IProductRepository
{
    private readonly Dictionary<Guid, Product> _products = [];
    public void AddProduct(Product product)
    {
        _products.Add(product.Id, product);
    }

    public Product GetById(Guid id)
    {
        _products.TryGetValue(id, out var product);
        return product;
    }
}
