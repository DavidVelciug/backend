var builder = WebApplication.CreateBuilder(args);

// 1. СЕРВИСЫ
// Временно убрали AddOpenApi(), чтобы исключить его как причину падения
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin() // Разрешаем временно всем, чтобы точно сработало
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 2. MIDDLEWARE
// ВАЖНО: Если запускаешь локально и есть проблемы с сертификатами, 
// можно временно закомментировать app.UseHttpsRedirection();
app.UseCors("DevCorsPolicy");

// 3. ЭНДПОИНТЫ
var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

app.MapGet("/", () => "API работает!"); // Проверочный эндпоинт на главной

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return Results.Ok(forecast);
});

app.Run();

// 4. МОДЕЛИ
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}