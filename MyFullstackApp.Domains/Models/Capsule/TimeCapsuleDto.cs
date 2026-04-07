using MyFullstackApp.Domains.Enums;

namespace MyFullstackApp.Domains.Models.Capsule;

public class TimeCapsuleDto
{
    public int Id { get; set; }
    public int OwnerUserId { get; set; }
    public string? OwnerDisplayName { get; set; }
    public CapsuleContentType ContentType { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TextContent { get; set; }
    public string? LinkUrl { get; set; }
    public string? FileStoragePath { get; set; }
    public DateTime OpenAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
}
