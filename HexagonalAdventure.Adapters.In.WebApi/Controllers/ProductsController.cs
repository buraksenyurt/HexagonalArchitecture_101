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
    public IActionResult Create([FromBody] CreateProductRequest request)
    {
        var productId = _productService.CreateProduct(request.Title, request.Price, request.Category, request.Stock);
        return Ok(new { Id = productId });
    }
}

public record CreateProductRequest(string Title, decimal Price, string Category, int Stock);
