namespace HexagonalAdventure.Application.Ports.Inbound;

public interface IProductService
{
    Guid CreateProduct(string title, string productCode, decimal price, Guid categoryId, int stock);
}
