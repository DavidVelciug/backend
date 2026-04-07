using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyFullstackApp.Domains.Entities.Moderation;
using MyFullstackApp.Domains.Entities.User;
using MyFullstackApp.Domains.Enums;

namespace MyFullstackApp.Domains.Entities.Capsule;

public class TimeCapsuleData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int OwnerUserId { get; set; }
    public UserAccountData Owner { get; set; } = null!;

    public CapsuleContentType ContentType { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? TextContent { get; set; }

    [StringLength(2000)]
    public string? LinkUrl { get; set; }

    [StringLength(500)]
    public string? FileStoragePath { get; set; }

    public DateTime OpenAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    [StringLength(256)]
    public string RecipientEmail { get; set; } = string.Empty;

    public bool IsPublic { get; set; }

    public CapsuleLocationData? Location { get; set; }
    public List<ModerationReportData> Reports { get; set; } = new();
}
