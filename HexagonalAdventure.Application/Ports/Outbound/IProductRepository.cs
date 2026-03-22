using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Application.Ports.Outbound;

public interface IProductRepository
{
    Task AddProductAsync(Product product);
    Task<Product> GetByIdAsync(Guid id);
    Task UpdateProductAsync(Product product);
}
