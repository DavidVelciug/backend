using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyFullstackApp.Domains.Entities.Capsule;
using MyFullstackApp.Domains.Enums;

namespace MyFullstackApp.Domains.Entities.Moderation;

public class ModerationReportData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CapsuleId { get; set; }
    public TimeCapsuleData Capsule { get; set; } = null!;

    [StringLength(256)]
    public string ReporterEmail { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Reason { get; set; } = string.Empty;

    public ReportStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
