using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Adapters.Out.EF;

public class EfCategoryRepository(DeppoDbContext deppoDbContext)
    : ICategoryRepository
{
    public void AddCategory(Category category)
    {
        deppoDbContext.Categories.Add(category);
        deppoDbContext.SaveChanges();
    }

    public IList<Category> GetAll()
    {
        return [.. deppoDbContext.Categories];
    }

    public Category GetById(Guid id)
    {
        return deppoDbContext.Categories.FirstOrDefault(c => c.Id == id);
    }
}
