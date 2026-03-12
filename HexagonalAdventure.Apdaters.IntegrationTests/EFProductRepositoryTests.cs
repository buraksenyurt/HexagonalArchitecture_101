using HexagonalAdventure.Domain;
using HexagonalAdventure.Adapters.Out.EF;
using Microsoft.EntityFrameworkCore;

namespace HexagonalAdventure.Apdaters.IntegrationTests;

public class EFProductRepositoryTests
{
    [Fact]
    public void Add_ShouldSaveProductToDatabase_And_GetById_ShouldReturnIt()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DeppoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new DeppoDbContext(options);
        var repository = new EfProductRepository(context);
        var productId = Guid.NewGuid();
        var productToSave = new Product(productId, "Learning the Hexagonal Architecture", 29.99m, "Books", 2);

        // Act
        repository.AddProduct(productToSave);

        // Assert
        var retrievedProduct = repository.GetById(productId);
        Assert.NotNull(retrievedProduct);
        Assert.Equal(productId, retrievedProduct.Id);
        Assert.Equal("Learning the Hexagonal Architecture", retrievedProduct.Title);
        Assert.Equal(29.99m, retrievedProduct.ListPrice);
        Assert.Equal("Books", retrievedProduct.Category);
        Assert.Equal(2, retrievedProduct.StockQuantity);
    }
}
