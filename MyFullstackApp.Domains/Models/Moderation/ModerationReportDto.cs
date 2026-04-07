using MyFullstackApp.Domains.Enums;

namespace MyFullstackApp.Domains.Models.Moderation;

public class ModerationReportDto
{
    public int Id { get; set; }
    public int CapsuleId { get; set; }
    public string ReporterEmail { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public ReportStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
