using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Application.Ports.Inbound;

public interface IProductService
{
    Task<Guid> CreateProduct(string title, string productCode, decimal price, Guid categoryId, int stock);
    Task IncreaseProductStock(Guid productId, int value);
    Task<Product> GetProductById(Guid productId);
}
