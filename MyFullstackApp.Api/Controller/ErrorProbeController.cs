using Microsoft.AspNetCore.Mvc;

namespace MyApi.Controller;

[Route("api/errorprobe")]
[ApiController]
public class ErrorProbeController : ControllerBase
{
    [HttpGet("{code:int}")]
    public IActionResult Probe(int code)
    {
        return code switch
        {
            401 => Unauthorized(),
            402 => StatusCode(402),
            403 => Forbid(),
            404 => NotFound(),
            500 => StatusCode(500),
            _ => Ok(new { code })
        };
    }
}
