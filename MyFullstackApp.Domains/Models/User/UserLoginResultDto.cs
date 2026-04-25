namespace MyFullstackApp.Domains.Models.User;

public class UserLoginResultDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public string Role { get; set; } = "guest";
    public string? DisplayName { get; set; }
}
