namespace HexagonalAdventure.Domain.UnitTests;

public class ProductTests
{
    [Fact]
    public void DecreaseStock_When_StockIsEnough()
    {
        // Arrange (Hazırlık safhası)
        var productCode = new ProductCode("MOUSE-001");
        var product = new Product(Guid.NewGuid(), productCode, "Optical Mouse", 29.99m, Guid.NewGuid(), 10);

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
        var productCode = new ProductCode("KEYBOARD-001");
        var product = new Product(Guid.NewGuid(), productCode, "Mechanical Keyboard", 79.99m, Guid.NewGuid(), 3);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => product.DecreaseStock(5));
    }

    [Fact]
    public void IncreaseStock_ShouldIncreaseStockQuantity()
    {
        // Arrange
        var productCode = new ProductCode("HEADSET-001");
        var product = new Product(Guid.NewGuid(), productCode, "Gaming Headset", 49.99m, Guid.NewGuid(), 5);

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
        var productCode = new ProductCode("HUB-001");
        var product = new Product(Guid.NewGuid(), productCode, "USB-C Hub", 39.99m, Guid.NewGuid(), 8);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.IncreaseStock(-5));
    }
}
