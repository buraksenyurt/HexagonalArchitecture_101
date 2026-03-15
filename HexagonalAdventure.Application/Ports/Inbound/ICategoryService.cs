namespace HexagonalAdventure.Application.Ports.Inbound;

public interface ICategoryService
{
    Guid CreateCategory(string name);
}
