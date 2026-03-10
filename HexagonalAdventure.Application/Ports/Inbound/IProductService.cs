namespace HexagonalAdventure.Application.Ports.Inbound;

public interface IProductService
{
    Guid CreateProduct(string title, decimal price, string category, int stock);
}
