using Microsoft.AspNetCore.Mvc;

namespace MyApi.Controller;

[Route("api/health")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => Ok(new { status = "ok", timeUtc = DateTime.UtcNow });
}
