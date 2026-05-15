namespace MyApi.Services;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "MemoryLane";
    public string Audience { get; set; } = "MemoryLaneClients";
    public int AccessMinutes { get; set; } = 60;
    public int RefreshDays { get; set; } = 14;
}
