namespace MyFullstackApp.Domains.Models.User;

public class UserAccountDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public bool NotifyEmailEnabled { get; set; }
    public bool NotifyPushEnabled { get; set; }
    public bool LoginAlertsEnabled { get; set; }
}
