namespace HexagonalAdventure.Domain.UnitTests;

public class ProductTests
{
    [Fact]
    public void DecreaseStock_When_StockIsEnough()
    {
        // Arrange (Hazırlık safhası)
        var product = new Product(Guid.NewGuid(), "Optical Mouse", 29.99m, "Electronics", 10);

        // Act (Eylem safhası)
        product.DecreaseStock(5);

        // Assert (Doğrulama safhası)
        var expectedStock = 5;
        Assert.Equal(expectedStock, product.StockQuantity);
    }

    [Fact]
    public void DecreaseStock_When_StockIsNotEnough_ShouldThrowException()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Mechanical Keyboard", 79.99m, "Electronics", 3);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => product.DecreaseStock(5));
    }

    [Fact]
    public void IncreaseStock_ShouldIncreaseStockQuantity()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Gaming Headset", 49.99m, "Electronics", 5);

        // Act
        product.IncreaseStock(10);

        // Assert
        var expectedStock = 15;
        Assert.Equal(expectedStock, product.StockQuantity);
    }

    [Fact]
    public void IncreaseStock_When_AmountIsNegative_ShouldThrowException()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "USB-C Hub", 39.99m, "Electronics", 8);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.IncreaseStock(-5));
    }
}
