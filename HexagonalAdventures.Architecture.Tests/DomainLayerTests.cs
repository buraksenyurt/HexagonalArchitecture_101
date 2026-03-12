using HexagonalAdventure.Application.Services;
using HexagonalAdventure.Domain;
using NetArchTest.Rules;

namespace HexagonalAdventures.Architecture.Tests;

public class DomainLayerTests
{
    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOnOtherLayers()
    {
        // Arrange
        var domainAssembly = typeof(Product).Assembly;

        // Act
        var result = Types.InAssembly(domainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
            "HexagonalAdventure.Application",
            "HexagonalAdventure.Adapters",
            "Microsoft.EntityFrameworkCore"
            )
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Domain layer should not have dependencies on Application, Adapters, or EF Core.");
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOnAdapters()
    {
        // Arrange
        var appAssembly = typeof(ProductService).Assembly;
        
        // Act
        var result = Types.InAssembly(appAssembly)
            .ShouldNot()
            .HaveDependencyOn("HexagonalAdventure.Adapters")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Application layer should not have dependencies on Adapters.");
    }
}
