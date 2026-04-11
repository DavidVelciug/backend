using Microsoft.AspNetCore.Mvc;
using MyFullstackApp.BusinessLogic;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.User;

namespace MyApi.Controller;

[Route("api/user")]
[ApiController]
public class UserAccountController : ControllerBase
{
    private readonly IUserAccount _users;

    public UserAccountController(BusinessLogic businessLogic)
    {
        _users = businessLogic.GetUserAccountActions();
    }

    [HttpGet("getAll")]
    public IActionResult GetAll()
    {
        return Ok(_users.GetAllUserAccountsAction());
    }

    [HttpGet("id")]
    public IActionResult Get(int id)
    {
        var u = _users.GetUserAccountByIdAction(id);
        return u == null ? NotFound() : Ok(u);
    }

    [HttpPost]
    public IActionResult Create([FromBody] UserAccountDto user)
    {
        return Ok(_users.ResponceUserAccountCreateAction(user));
    }

    [HttpPut]
    public IActionResult Update([FromBody] UserAccountDto user)
    {
        return Ok(_users.ResponceUserAccountUpdateAction(user));
    }

    [HttpDelete("id")]
    public IActionResult Delete(int id)
    {
        return Ok(_users.ResponceUserAccountDeleteAction(id));
    }
}
