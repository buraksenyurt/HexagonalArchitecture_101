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

    [HttpPost("{id}/stock/increase")]
    public async Task<IActionResult> IncreaseStock(Guid id, [FromBody] IncreaseStockRequest request)
    {
        //todo@buraksenyurt Global Exception Handler ekleyelim ya da 404'ü yakalayıp dönelim
        await _productService.IncreaseProductStock(id, request.Value);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetProductById(id);
        var response = new GetProductResponse(
            product.Title,
            product.Code.Value,
            product.ListPrice,
            product.CategoryId,
            product.StockQuantity);
        return Ok(response);
    }
}

public record CreateProductRequest(string Title, string ProductCode, decimal Price, Guid CategoryId, int Stock);

public record IncreaseStockRequest(int Value);

public record GetProductResponse(string Title, string ProductCode, decimal Price, Guid CategoryId, int Stock);
