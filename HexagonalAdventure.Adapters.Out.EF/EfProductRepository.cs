using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain;
using Microsoft.EntityFrameworkCore;

namespace HexagonalAdventure.Adapters.Out.EF;

public class EfProductRepository(DeppoDbContext deppoDbContext)
    : IProductRepository
{
    public async Task AddProductAsync(Product product)
    {
        deppoDbContext.Products.Add(product);
        await deppoDbContext.SaveChangesAsync();
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await deppoDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateProductAsync(Product product)
    {
        await deppoDbContext.SaveChangesAsync();
    }
}
