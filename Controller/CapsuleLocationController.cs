using Microsoft.AspNetCore.Mvc;
using MyFullstackApp.BusinessLogic;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.Capsule;

namespace MyApi.Controller;

[Route("api/capsulelocation")]
[ApiController]
public class CapsuleLocationController : ControllerBase
{
    private readonly ICapsuleLocation _locations;

    public CapsuleLocationController(BusinessLogic businessLogic)
    {
        _locations = businessLogic.GetCapsuleLocationActions();
    }

    [HttpGet("getAll")]
    public IActionResult GetAll()
    {
        return Ok(_locations.GetAllCapsuleLocationsAction());
    }

    [HttpGet("id")]
    public IActionResult Get(int id)
    {
        var l = _locations.GetCapsuleLocationByIdAction(id);
        return l == null ? NotFound() : Ok(l);
    }

    [HttpGet("byCapsule")]
    public IActionResult GetByCapsule(int capsuleId)
    {
        var l = _locations.GetCapsuleLocationByCapsuleIdAction(capsuleId);
        return l == null ? NotFound() : Ok(l);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CapsuleLocationDto location)
    {
        return Ok(_locations.ResponceCapsuleLocationCreateAction(location));
    }

    [HttpPut]
    public IActionResult Update([FromBody] CapsuleLocationDto location)
    {
        return Ok(_locations.ResponceCapsuleLocationUpdateAction(location));
    }

    [HttpDelete("id")]
    public IActionResult Delete(int id)
    {
        return Ok(_locations.ResponceCapsuleLocationDeleteAction(id));
    }
}