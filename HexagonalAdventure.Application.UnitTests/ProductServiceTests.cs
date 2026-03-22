namespace HexagonalAdventure.Application.UnitTests;

using Moq;
using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Application.Services;
using HexagonalAdventure.Domain;
using HexagonalAdventure.Application.Events;
using HexagonalAdventure.Domain.Events;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateProduct_ShouldReturnValidGuid()
    {
        // Arange
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(r => r.AddProductAsync(It.IsAny<Product>()));
        var mockEventDispatcher = new Mock<IEventDispatcher>();

        // Act
        var service = new ProductService(mockRepo.Object, mockEventDispatcher.Object);
        var actualGuid = await service.CreateProduct("AyBiEm Laptop i7", "LAPTOP-1012", 1500m, Guid.NewGuid(), 10);

        // Assert
        Assert.NotEqual(Guid.Empty, actualGuid);

        // Verify (Gerçekten de dış bağımlılıktaki AddProduct metodunun çağrıldığını doğrulamak için)
        mockRepo.Verify(r => r.AddProductAsync(It.IsAny<Product>()), Times.Once);
        mockEventDispatcher.Verify(e => e.DispatchAsync(It.IsAny<ProductCreatedEvent>()), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_ShouldPassCorrectDataToRepository()
    {
        // Arange
        var mockRepo = new Mock<IProductRepository>();
        Product capturedProduct = null;
        mockRepo.Setup(r => r.AddProductAsync(It.IsAny<Product>()))
                .Callback<Product>(p => capturedProduct = p);
        var categoryId = Guid.NewGuid();
        var mockEventDispatcher = new Mock<IEventDispatcher>();

        // Act
        var service = new ProductService(mockRepo.Object, mockEventDispatcher.Object);
        var actualGuid = await service.CreateProduct("AyBiEm Laptop i7", "LAPTOP-1012", 1500m, categoryId, 10);

        // Assert
        Assert.NotNull(capturedProduct);
        Assert.Equal("AyBiEm Laptop i7", capturedProduct.Title);
        Assert.Equal("LAPTOP-1012", capturedProduct.Code.Value);
        Assert.Equal(1500m, capturedProduct.ListPrice);
        Assert.Equal(categoryId, capturedProduct.CategoryId);
        Assert.Equal(10, capturedProduct.StockQuantity);

        // Verify
        mockEventDispatcher.Verify(e => e.DispatchAsync(It.IsAny<ProductCreatedEvent>()), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_WhenTitleIsEmpty_ShouldThrowException()
    {
        // Arange
        var mockRepo = new Mock<IProductRepository>();
        var mockEventDispatcher = new Mock<IEventDispatcher>();

        // Act
        var service = new ProductService(mockRepo.Object, mockEventDispatcher.Object);
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateProduct("", "LAPTOP-1023", 1500m, Guid.NewGuid(), 10)
        );

        // Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(
            () => service.CreateProduct("", "LAPTOP-1023", 1500m, Guid.NewGuid(), 10)
        );
        Assert.Equal("Title cannot be empty", exception.Message);

        // Verify
        mockEventDispatcher.Verify(e => e.DispatchAsync(It.IsAny<IDomainEvent>()), Times.Never);
    }

    [Fact]
    public async Task CreateProduct_WithNegativeStockQuantity_ShouldThrowException()
    {
        //Arange
        var mockRepo = new Mock<IProductRepository>();
        var mockEventDispatcher = new Mock<IEventDispatcher>();
        var service = new ProductService(mockRepo.Object, mockEventDispatcher.Object);

        //Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateProduct("AyBiEm Laptop i7", "LAPTOP-1012", 1500m, Guid.NewGuid(), -5)
        );

        //Verify
        mockRepo.Verify(r => r.AddProductAsync(It.IsAny<Product>()), Times.Never);
        mockEventDispatcher.Verify(e => e.DispatchAsync(It.IsAny<IDomainEvent>()), Times.Never);
    }
}
