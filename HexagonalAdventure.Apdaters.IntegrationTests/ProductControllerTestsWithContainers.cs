using HexagonalAdventure.Adapters.In.WebApi.Controllers;
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
        var client = factory.CreateClient();
        var request = new CreateProductRequest("Pragmatic Programmer", 42.99m, "Books", 4);

        // Act
        var response = await client.PostAsJsonAsync("/api/products", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseData = await response.Content.ReadFromJsonAsync<CreateProductResponse>();
        Assert.NotNull(responseData);
        Assert.NotEqual(Guid.Empty, responseData.Id);
    }
}
