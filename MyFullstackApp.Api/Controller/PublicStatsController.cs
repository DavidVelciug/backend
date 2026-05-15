using Microsoft.AspNetCore.Mvc;
using MyFullstackApp.DataAccess.Context;

namespace MyApi.Controller;

[Route("api/stats")]
[ApiController]
public class PublicStatsController : ControllerBase
{
    [HttpGet("capsuleCounts")]
    public IActionResult CapsuleCounts()
    {
        using var db = new AppDbContext();
        var now = DateTime.UtcNow;
        var createdTotal = db.TimeCapsules.Count();
        var openedTotal = db.TimeCapsules.Count(c => c.OpenAtUtc <= now);
        return Ok(new { createdTotal, openedTotal });
    }
}
