using Microsoft.AspNetCore.Mvc;
using MyFullstackApp.BusinessLogic;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.Capsule;

namespace MyApi.Controller;

[Route("api/timecapsule")]
[ApiController]
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

    [HttpGet("getByOwner")]
    public IActionResult GetByOwner(int ownerUserId)
    {
        return Ok(_capsules.GetTimeCapsulesByOwnerAction(ownerUserId));
    }

    [HttpGet("getPublicFeed")]
    public IActionResult GetPublicFeed()
    {
        return Ok(_capsules.GetPublicFeedAction());
    }

    [HttpPost]
    public IActionResult Create([FromBody] TimeCapsuleDto capsule)
    {
        return Ok(_capsules.ResponceTimeCapsuleCreateAction(capsule));
    }

    [HttpPut]
    public IActionResult Update([FromBody] TimeCapsuleDto capsule)
    {
        return Ok(_capsules.ResponceTimeCapsuleUpdateAction(capsule));
    }

    [HttpDelete("id")]
    public IActionResult Delete(int id)
    {
        return Ok(_capsules.ResponceTimeCapsuleDeleteAction(id));
    }
}