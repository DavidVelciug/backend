namespace MyFullstackApp.DataAccess;

public static class DbSession
{
    public static string ConnectionStrings { get; set; } = string.Empty;
    public static string Provider { get; set; } = "sqlite";
}
