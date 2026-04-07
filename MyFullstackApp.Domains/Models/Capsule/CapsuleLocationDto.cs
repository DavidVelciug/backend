namespace MyFullstackApp.Domains.Models.Capsule;

public class CapsuleLocationDto
{
    public int Id { get; set; }
    public int CapsuleId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string PlaceLabel { get; set; } = string.Empty;
}
