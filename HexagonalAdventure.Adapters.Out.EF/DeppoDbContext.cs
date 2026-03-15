using HexagonalAdventure.Domain;
using Microsoft.EntityFrameworkCore;

namespace HexagonalAdventure.Adapters.Out.EF;

public class DeppoDbContext(DbContextOptions<DeppoDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Code)
                .HasConversion(
                    v => v.Value, // ProductCode'u string'e dönüştürme
                    v => new ProductCode(v)) // string'i ProductCode'a dönüştürme
                .IsRequired()
                .HasMaxLength(20);

            entity.HasOne<Category>()
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Kategori silindiğinde ürünlerin silinmemesi için Restrict kullanıyoruz
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
        });
    }
}
