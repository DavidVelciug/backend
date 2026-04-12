using Microsoft.AspNetCore.Mvc;
using MyFullstackApp.BusinessLogic;
using MyFullstackApp.BusinessLogic.Interface;
using MyFullstackApp.Domains.Models.Moderation;

namespace MyApi.Controller;

[Route("api/moderationreport")]
[ApiController]
public class ModerationReportController : ControllerBase
{
    private readonly IModerationReport _reports;

    public ModerationReportController(BusinessLogic businessLogic)
    {
        _reports = businessLogic.GetModerationReportActions();
    }

    [HttpGet("getAll")]
    public IActionResult GetAll()
    {
        return Ok(_reports.GetAllModerationReportsAction());
    }

    [HttpGet("id")]
    public IActionResult Get(int id)
    {
        var r = _reports.GetModerationReportByIdAction(id);
        return r == null ? NotFound() : Ok(r);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ModerationReportDto report)
    {
        return Ok(_reports.ResponceModerationReportCreateAction(report));
    }

    [HttpPut]
    public IActionResult Update([FromBody] ModerationReportDto report)
    {
        return Ok(_reports.ResponceModerationReportUpdateAction(report));
    }

    [HttpDelete("id")]
    public IActionResult Delete(int id)
    {
        return Ok(_reports.ResponceModerationReportDeleteAction(id));
    }
}
