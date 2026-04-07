namespace MyFullstackApp.Domains.Models.Admin;

public class TimeSeriesPointDto
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
}

public class AdminStatsDto
{
    public List<TimeSeriesPointDto> UserRegistrationsByDay { get; set; } = new();
    public List<TimeSeriesPointDto> CapsulesCreatedByDay { get; set; } = new();
}
