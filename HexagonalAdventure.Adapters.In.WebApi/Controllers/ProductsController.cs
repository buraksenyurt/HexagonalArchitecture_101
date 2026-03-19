using HexagonalAdventure.Application.Ports.Inbound;
using Microsoft.AspNetCore.Mvc;

namespace HexagonalAdventure.Adapters.In.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService)
    : Controller
{
    private readonly IProductService _productService = productService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var productId = await _productService.CreateProduct(
            request.Title
            , request.ProductCode
            , request.Price
            , request.CategoryId
            , request.Stock);
        return Ok(new { Id = productId });
    }
}

public record CreateProductRequest(string Title, string ProductCode, decimal Price, Guid CategoryId, int Stock);
