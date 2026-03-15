namespace HexagonalAdventure.Application.UnitTests;

using Moq;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Application.Services;
using HexagonalAdventure.Domain;

public class ProductServiceTests
{
    [Fact]
    public void CreateProduct_ShouldReturnValidGuid()
    {
        // Arange
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(r => r.AddProduct(It.IsAny<Domain.Product>()));

        // Act
        var service = new ProductService(mockRepo.Object);
        var actualGuid = service.CreateProduct("AyBiEm Laptop i7", "LAPTOP-1012", 1500m, Guid.NewGuid(), 10);

        // Assert
        Assert.NotEqual(Guid.Empty, actualGuid);

        // Verify (Gerçekten de dış bağımlılıktaki AddProduct metodunun çağrıldığını doğrulamak için)
        mockRepo.Verify(r => r.AddProduct(It.IsAny<Domain.Product>()), Times.Once);
    }

    [Fact]
    public void CreateProduct_ShouldPassCorrectDataToRepository()
    {
        // Arange
        var mockRepo = new Mock<IProductRepository>();
        Product capturedProduct = null;
        mockRepo.Setup(r => r.AddProduct(It.IsAny<Product>()))
                .Callback<Product>(p => capturedProduct = p);
        var categoryId = Guid.NewGuid();

        // Act
        var service = new ProductService(mockRepo.Object);
        var actualGuid = service.CreateProduct("AyBiEm Laptop i7", "LAPTOP-1012", 1500m, categoryId, 10);

        // Assert
        Assert.NotNull(capturedProduct);
        Assert.Equal("AyBiEm Laptop i7", capturedProduct.Title);
        Assert.Equal("LAPTOP-1012", capturedProduct.Code.Value);
        Assert.Equal(1500m, capturedProduct.ListPrice);
        Assert.Equal(categoryId, capturedProduct.CategoryId);
        Assert.Equal(10, capturedProduct.StockQuantity);
    }

    [Fact]
    public void CreateProduct_WhenTitleIsEmpty_ShouldThrowException()
    {
        // Arange
        var mockRepo = new Mock<IProductRepository>();

        // Act
        var service = new ProductService(mockRepo.Object);

        // Assert
        Assert.Throws<ArgumentException>(() => service.CreateProduct("", "LAPTOP-1023", 1500m, Guid.NewGuid(), 10));
    }

    [Fact]
    public void CreateProduct_WithNegativeStockQuantity_ShouldThrowException()
    {
        //Arange
        var mockRepo = new Mock<IProductRepository>();
        var service = new ProductService(mockRepo.Object);

        //Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => service.CreateProduct("AyBiEm Laptop i7", "LAPTOP-1023", 1500m, Guid.NewGuid(), -5));

        //Verify
        mockRepo.Verify(r => r.AddProduct(It.IsAny<Product>()), Times.Never);
    }
}
