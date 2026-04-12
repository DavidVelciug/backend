using Microsoft.AspNetCore.Mvc;
using MyFullstackApp.BusinessLogic;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.Category;

namespace MyApi.Controller;

[Route("api/category")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategory _category;

    public CategoryController(BusinessLogic businessLogic)
    {
        _category = businessLogic.GetCategoryActions();
    }

    [HttpGet("getAll")]
    public IActionResult GetAllCategories()
    {
        var categories = _category.GetAllCategoriesAction();
        return Ok(categories);
    }

    [HttpGet("id")]
    public IActionResult Get(int id)
    {
        var category = _category.GetCategoryByIdAction(id);
        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CategoryDto category)
    {
        var status = _category.ResponceCategoryCreateAction(category);
        return Ok(status);
    }

    [HttpPut]
    public IActionResult Update([FromBody] CategoryDto category)
    {
        var status = _category.ResponceCategoryUpdateAction(category);
        return Ok(status);
    }

    [HttpDelete("id")]
    public IActionResult Delete(int id)
    {
        var status = _category.ResponceCategoryDeleteAction(id);
        return Ok(status);
    }
}
