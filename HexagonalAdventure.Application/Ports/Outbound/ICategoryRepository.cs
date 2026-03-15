using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Application.Ports.Outbound;

public interface ICategoryRepository
{
    void AddCategory(Category category);
}
