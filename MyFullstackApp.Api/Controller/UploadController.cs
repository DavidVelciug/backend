using Microsoft.AspNetCore.Mvc;
using MyApi.Filters;
using MyFullstackApp.BusinessLogic.Core.Common;

namespace MyApi.Controller;

[Route("api/upload")]
[ApiController]
[RoleAccess(AppRoles.User, AppRoles.Moderator, AppRoles.Admin)]
public class UploadController : ControllerBase
{
    [HttpPost("file")]
    [RequestSizeLimit(long.MaxValue)]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public IActionResult UploadFile([FromForm] IFormFile? file, [FromQuery] string? folder = null)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { isSuccess = false, message = "Файл не передан." });
        }

        var safeFolder = string.IsNullOrWhiteSpace(folder) ? "misc" : folder.Trim().ToLowerInvariant();
        if (safeFolder.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || safeFolder.Contains("..", StringComparison.Ordinal))
        {
            return BadRequest(new { isSuccess = false, message = "Некорректная папка загрузки." });
        }

        using var stream = file.OpenReadStream();
        var path = ImageStorage.SaveUploadedFile(stream, file.FileName, safeFolder);

        return Ok(new { isSuccess = true, message = "Файл загружен.", path });
    }
}
