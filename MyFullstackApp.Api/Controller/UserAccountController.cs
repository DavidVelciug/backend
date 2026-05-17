using Microsoft.AspNetCore.Mvc;
using MyApi.Filters;
using MyApi.Services;
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
    private readonly JwtTokenService _jwt;

    public UserAccountController(BusinessLogic businessLogic, JwtTokenService jwt)
    {
        _users = businessLogic.GetUserAccountActions();
        _jwt = jwt;
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
        var result = _users.LoginUserAction(request);
        if (result is { IsSuccess: true, UserId: not null })
        {
            var user = _users.GetUserAccountByIdAction(result.UserId!.Value);
            if (user != null)
            {
                var tokens = _jwt.CreateTokens(user.Id, user.Role, user.DisplayName, user.Email);
                result.AccessToken = tokens.AccessToken;
                result.RefreshToken = tokens.RefreshToken;
                result.AccessExpiresUtc = tokens.AccessExpiresUtc;
                result.Email = user.Email;
            }
        }

        return Ok(result);
    }

    [HttpPost("refresh")]
    [RoleAccess(AppRoles.Guest, AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult Refresh([FromBody] RefreshTokenRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Unauthorized();
        }

        var userId = _jwt.ValidateRefreshTokenAndGetUserId(request.RefreshToken);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = _users.GetUserAccountByIdAction(userId.Value);
        if (user == null)
        {
            return Unauthorized();
        }

        var tokens = _jwt.CreateTokens(user.Id, user.Role, user.DisplayName, user.Email);
        return Ok(new UserLoginResultDto
        {
            IsSuccess = true,
            Message = "Токен обновлён.",
            UserId = user.Id,
            Role = user.Role,
            DisplayName = user.DisplayName,
            Email = user.Email,
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            AccessExpiresUtc = tokens.AccessExpiresUtc,
        });
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