using HexagonalAdventure.Domain;
using Microsoft.EntityFrameworkCore;

namespace HexagonalAdventure.Adapters.Out.EF;

public class DeppoDbContext(DbContextOptions<DeppoDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}
