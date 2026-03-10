using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain;

namespace HexagonalAdventure.Adapters.Out.EF;

public class EfProductRepository(DeppoDbContext deppoDbContext)
    : IProductRepository
{
    public void AddProduct(Product product)
    {
        deppoDbContext.Products.Add(product);
        deppoDbContext.SaveChanges();
    }

    public Product GetById(Guid id)
    {
        return deppoDbContext.Products.FirstOrDefault(p => p.Id == id);
    }
}
