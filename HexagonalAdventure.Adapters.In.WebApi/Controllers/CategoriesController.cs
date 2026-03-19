using HexagonalAdventure.Application.Ports.Inbound;
using Microsoft.AspNetCore.Mvc;

namespace HexagonalAdventure.Adapters.In.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService)
    : Controller
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpPost]
    public IActionResult Create([FromBody] CreateCategoryRequest request)
    {
        var categoryId = _categoryService.CreateCategory(request.Name);
        return Ok(new { Id = categoryId });
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_categoryService.GetAllCategories());
    }
}

public record CreateCategoryRequest(string Name);
