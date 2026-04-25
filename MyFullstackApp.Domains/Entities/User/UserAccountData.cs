using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyFullstackApp.Domains.Entities.Capsule;

namespace MyFullstackApp.Domains.Entities.User;

public class UserAccountData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [StringLength(120)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Role { get; set; } = "user";

    [Required]
    [StringLength(200)]
    public string Password { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public bool NotifyEmailEnabled { get; set; } = true;
    public bool NotifyPushEnabled { get; set; } = true;
    public bool LoginAlertsEnabled { get; set; } = true;

    public List<TimeCapsuleData> Capsules { get; set; } = new();
}
