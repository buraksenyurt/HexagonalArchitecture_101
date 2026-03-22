using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Adapters.Out.InMemory;

public class InMemoryProdutRepository
    : IProductRepository
{
    private readonly Dictionary<Guid, Product> _products = [];
    public Task AddProductAsync(Product product)
    {
        _products.Add(product.Id, product);
        return Task.CompletedTask;
    }

    public Task<Product> GetByIdAsync(Guid id)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task UpdateProductAsync(Product product)
    {
        if (!_products.ContainsKey(product.Id))
        {
            throw new KeyNotFoundException("Product not found with the specified ID");
        }

        _products[product.Id] = product;

        return Task.CompletedTask;
    }
}
