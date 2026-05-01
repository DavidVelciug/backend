using Microsoft.AspNetCore.Mvc;
using MyApi.Filters;
using MyFullstackApp.BusinessLogic;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.Capsule;

namespace MyApi.Controller;

[Route("api/timecapsule")]
[ApiController]
[RoleAccess(AppRoles.Guest, AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
public class TimeCapsuleController : ControllerBase
{
    private readonly ITimeCapsule _capsules;

    public TimeCapsuleController(BusinessLogic businessLogic)
    {
        _capsules = businessLogic.GetTimeCapsuleActions();
    }

    [HttpGet("getAll")]
    public IActionResult GetAll()
    {
        return Ok(_capsules.GetAllTimeCapsulesAction());
    }

    [HttpGet("id")]
    public IActionResult Get(int id)
    {
        var c = _capsules.GetTimeCapsuleByIdAction(id);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpGet("idForUser")]
    [RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult GetForUser(int id, int viewerUserId)
    {
        var c = _capsules.GetTimeCapsuleByIdForUserAction(id, viewerUserId);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpGet("getByOwner")]
    [RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult GetByOwner(int ownerUserId)
    {
        return Ok(_capsules.GetTimeCapsulesByOwnerAction(ownerUserId));
    }

    [HttpGet("getPublicFeed")]
    public IActionResult GetPublicFeed()
    {
        return Ok(_capsules.GetPublicFeedAction());
    }

    [HttpGet("getOpenedForUser")]
    [RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult GetOpenedForUser(int userId)
    {
        return Ok(_capsules.GetOpenedCapsulesForUserAction(userId));
    }

    [HttpPost]
    [RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult Create([FromBody] TimeCapsuleDto capsule)
    {
        return Ok(_capsules.ResponceTimeCapsuleCreateAction(capsule));
    }

    [HttpPut]
    [RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult Update([FromBody] TimeCapsuleDto capsule)
    {
        return Ok(_capsules.ResponceTimeCapsuleUpdateAction(capsule));
    }

    [HttpDelete("id")]
    [RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult Delete(int id)
    {
        return Ok(_capsules.ResponceTimeCapsuleDeleteAction(id));
    }

    [HttpDelete("owner")]
    [RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
    public IActionResult DeleteByOwner(int id, int ownerUserId)
    {
        return Ok(_capsules.ResponceTimeCapsuleDeleteByOwnerAction(id, ownerUserId));
    }
}
