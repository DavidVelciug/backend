using Microsoft.AspNetCore.Mvc;
using MyApi.Filters;
using MyFullstackApp.BusinessLogic;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.User;

namespace MyApi.Controller;

[Route("api/user")]
[ApiController]
[RoleAccess(AppRoles.Guest, AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
public class UserAccountController : ControllerBase
{
    private readonly IUserAccount _users;

    public UserAccountController(BusinessLogic businessLogic)
    {
        _users = businessLogic.GetUserAccountActions();
    }

    [HttpGet("getAll")]
    [RoleAccess(AppRoles.Admin, AppRoles.Moderator)]
    public IActionResult GetAll()
    {
        return Ok(_users.GetAllUserAccountsAction());
    }

    [HttpGet("id")]
    [RoleAccess(AppRoles.Guest, AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult Get(int id)
    {
        var u = _users.GetUserAccountByIdAction(id);
        return u == null ? NotFound() : Ok(u);
    }

    [HttpPost("login")]
    [RoleAccess(AppRoles.Guest, AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult Login([FromBody] UserLoginRequestDto request)
    {
        return Ok(_users.LoginUserAction(request));
    }

    [HttpPost]
    [RoleAccess(AppRoles.Guest, AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult Create([FromBody] UserAccountDto user)
    {
        return Ok(_users.ResponceUserAccountCreateAction(user));
    }

    [HttpPut]
    [RoleAccess(AppRoles.User, AppRoles.Admin)]
    public IActionResult Update([FromBody] UserAccountDto user)
    {
        return Ok(_users.ResponceUserAccountUpdateAction(user));
    }

    [HttpDelete("id")]
    [RoleAccess(AppRoles.Admin)]
    public IActionResult Delete(int id)
    {
        return Ok(_users.ResponceUserAccountDeleteAction(id));
    }
}