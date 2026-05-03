using Microsoft.EntityFrameworkCore;
using MyFullstackApp.BusinessLogic.Mapping;
using MyFullstackApp.DataAccess;
using MyFullstackApp.DataAccess.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// 1. Настройка сессии базы данных
ConfigureDbSession(builder);

// 2. Регистрация DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (DbSession.Provider == "sqlserver")
    {
        options.UseSqlServer(DbSession.ConnectionStrings, b => 
            b.MigrationsAssembly("MyFullstackApp.DataAccess"));
    }
    else
    {
        options.UseSqlite(DbSession.ConnectionStrings, b => 
            b.MigrationsAssembly("MyFullstackApp.DataAccess"));
    }
});

// 3. Настройка аутентификации 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped<MyFullstackApp.BusinessLogic.BusinessLogic>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Инициализация базы данных
InitializeDatabase(app);

app.UseCors("DevCorsPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();

// ПОРЯДОК ВАЖЕН: Сначала КТО ты (Authn), потом ЧТО тебе можно (Authz)
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();

static void InitializeDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate();
        DbInitializer.SeedIfEmpty(db);
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при инициализации базы данных.");
    }
}

static void ConfigureDbSession(WebApplicationBuilder builder)
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(conn)) conn = "Data Source=memorylane.db";

    var isSqlServer = conn.Contains("Server=", StringComparison.OrdinalIgnoreCase) || 
                      conn.Contains("Host=", StringComparison.OrdinalIgnoreCase);

    if (isSqlServer)
    {
        DbSession.ConnectionStrings = conn;
        DbSession.Provider = "sqlserver";
    }
    else
    {
        const string prefix = "Data Source=";
        var relative = conn.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ? conn[prefix.Length..].Trim() : conn;
        var path = Path.IsPathRooted(relative) ? relative : Path.Combine(builder.Environment.ContentRootPath, relative);
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
        DbSession.ConnectionStrings = $"Data Source={path}";
        DbSession.Provider = "sqlite";
    }
}