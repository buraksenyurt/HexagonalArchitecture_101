using HexagonalAdventure.Application.Ports.Inbound;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Application.Services;

public class CategoryService(ICategoryRepository categoryRepository)
    : ICategoryService
{
    public Guid CreateCategory(string name)
    {
        var category = new Category(Guid.NewGuid(), name);
        categoryRepository.AddCategory(category);
        return category.Id;
    }
}
