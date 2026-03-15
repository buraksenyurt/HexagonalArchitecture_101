using HexagonalAdventure.Adapters.In.WebApi.Controllers;
using HexagonalAdventure.Adapters.Out.EF;
using HexagonalAdventure.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace HexagonalAdventure.Apdaters.IntegrationTests;

public class ProductControllerTests(PostgresWebApplicationFactory factory)
    : IClassFixture<PostgresWebApplicationFactory>
{
    private record CreateProductResponse(Guid Id);

    [Fact]
    public async Task CreateProduct_WhenUsingContainer_ShouldReturn200OkWithProductId()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        
        // First, create a category in the database to satisfy foreign key constraint
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DeppoDbContext>();
            var category = new Category(categoryId, "Books");
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
        }
        
        var client = factory.CreateClient();
        var request = new CreateProductRequest("Pragmatic Programmer", "BOOK-1234", 42.99m, categoryId, 4);

        // Act
        var response = await client.PostAsJsonAsync("/api/products", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseData = await response.Content.ReadFromJsonAsync<CreateProductResponse>();
        Assert.NotNull(responseData);
        Assert.NotEqual(Guid.Empty, responseData.Id);
    }
}
