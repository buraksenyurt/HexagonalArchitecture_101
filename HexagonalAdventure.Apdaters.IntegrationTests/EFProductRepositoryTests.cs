using HexagonalAdventure.Domain;
using HexagonalAdventure.Adapters.Out.EF;
using Microsoft.EntityFrameworkCore;

namespace HexagonalAdventure.Apdaters.IntegrationTests;

public class EFProductRepositoryTests
{
    [Fact]
    public async Task Add_ShouldSaveProductToDatabase_And_GetById_ShouldReturnIt()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DeppoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new DeppoDbContext(options);
        var repository = new EfProductRepository(context);
        var productId = Guid.NewGuid();
        var productCode = new ProductCode("BOOK-001");
        var categoryId = Guid.NewGuid();
        var productToSave = new Product(productId, productCode, "Learning the Hexagonal Architecture", 29.99m, categoryId, 2);

        // Act
        await repository.AddProductAsync(productToSave);

        // Assert
        var retrievedProduct = await repository.GetByIdAsync(productId);
        Assert.NotNull(retrievedProduct);
        Assert.Equal(productId, retrievedProduct.Id);
        Assert.Equal(productCode, retrievedProduct.Code);
        Assert.Equal("Learning the Hexagonal Architecture", retrievedProduct.Title);
        Assert.Equal(29.99m, retrievedProduct.ListPrice);
        Assert.Equal(categoryId, retrievedProduct.CategoryId);
        Assert.Equal(2, retrievedProduct.StockQuantity);
    }
}
