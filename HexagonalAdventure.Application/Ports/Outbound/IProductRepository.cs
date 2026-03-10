using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Application.Ports.Outbound;

public interface IProductRepository
{
    void AddProduct(Product product);
    Product GetById(Guid id);
}
