using HexagonalAdventure.Adapters.In.WebApi.Controllers;
using HexagonalAdventure.Adapters.Out.EF;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;
using System.Net;
using System.Net.Http.Json;

namespace HexagonalAdventure.Apdaters.IntegrationTests;

public class ProductControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    // Program sınıfı Web API projesindeki sınıfımızdır.
    // WebApplicationFactory, bu sınıfı kullanarak testler için bir test sunucusu oluşturur.
    private record CreateProductResponse(Guid Id);

    [Fact]
    public async Task CreateProduct_ShouldReturn200OkWithProductId()
    {
        // Arrange
        // Program sınıfımızdaki DI servisi, DbContext türevini Postgresql ile çalışacak şekilde yapılandırıyor.
        // Tabbi EF kullandığımız için beraberinde de birçok servis enjekte ediliyor. Bu yüzden DbContext ile ilgili
        // ne kadar kayıtlı bileşen varsa kaldırıyoruz.
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<DeppoDbContext>)); // Program sınıfında AddDbContext'in kaydettiği Npgsql yapılandırma kaynağını kaldırır.
                services.RemoveAll(typeof(DbContextOptions<DeppoDbContext>)); // DbContext ile ilgili tüm servisleri kaldırır.
                services.RemoveAll(typeof(DbConnection)); // Varsa DbConnection ile ilgili tüm servisleri kaldırır. Örneğin veritabanı kayıtları silinir.

                services.AddDbContext<DeppoDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DbTest_" + Guid.NewGuid().ToString());
                });
            });
        }).CreateClient();
        var request = new CreateProductRequest("Pragmatic Programmer", 42.99m, "Books", 4);

        // Act
        var response = await client.PostAsJsonAsync("/api/products", request); // POST isteği gönderilir ve yanıt alınır.

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseData = await response.Content.ReadFromJsonAsync<CreateProductResponse>();
        Assert.NotNull(responseData);
        Assert.NotEqual(Guid.Empty, responseData.Id);
    }

    //[Fact]
    //public async Task CreateProduct_ShouldReturn200OkWithProductId()
    //{
    //    // Arrange
    //    var client = factory.CreateClient(); // Test sunucusuna istek göndermek için fabrikadan bir HttpClient oluşturulur.
    //    var request = new CreateProductRequest("Pragmatic Programmer", 42.99m, "Books", 4);

    //    // Act
    //    var response = await client.PostAsJsonAsync("/api/products", request); // POST isteği gönderilir ve yanıt alınır.

    //    // Assert
    //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //    var responseData = await response.Content.ReadFromJsonAsync<CreateProductResponse>();
    //    Assert.NotNull(responseData);
    //    Assert.NotEqual(Guid.Empty, responseData.Id);
    //}
}
