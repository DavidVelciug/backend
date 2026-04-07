using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFullstackApp.Domains.Entities.Capsule;

public class CapsuleLocationData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CapsuleId { get; set; }
    public TimeCapsuleData Capsule { get; set; } = null!;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    [StringLength(200)]
    public string PlaceLabel { get; set; } = string.Empty;
}
