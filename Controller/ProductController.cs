using Microsoft.AspNetCore.Mvc;
using MyApi.Filters;
using MyFullstackApp.BusinessLogic;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.Product;

namespace MyApi.Controller;

[Route("api/product")]
[ApiController]
[RoleAccess(AppRoles.Guest, AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
public class ProductController : ControllerBase
{
    private readonly IProduct _product;

    public ProductController(BusinessLogic businessLogic)
    {
        _product = businessLogic.GetProductActions();
    }

    [HttpGet("getAll")]
    public IActionResult GetAllProducts()
    {
        var products = _product.GetAllProductsAction();
        return Ok(products);
    }

    [HttpGet("id")]
    public IActionResult Get(int id)
    {
        var product = _product.GetProductByIdAction(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    [RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult Create([FromBody] ProductDto product)
    {
        var status = _product.ResponceProductCreateAction(product);
        return Ok(status);
    }

    [HttpPut]
    [RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult Update([FromBody] ProductDto product)
    {
        var status = _product.ResponceProductUpdateAction(product);
        return Ok(status);
    }

    [HttpDelete("id")]
    [RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult Delete(int id)
    {
        var status = _product.ResponceProductDeleteAction(id);
        return Ok(status);
    }
}
