using MyFullstackApp.DataAccess.Context;
using MyFullstackApp.Domains.Entities.Capsule;
using MyFullstackApp.Domains.Entities.Category;
using MyFullstackApp.Domains.Entities.Moderation;
using MyFullstackApp.Domains.Entities.Product;
using MyFullstackApp.Domains.Entities.User;
using MyFullstackApp.Domains.Enums;

namespace MyFullstackApp.DataAccess;

public static class DbInitializer
{
    public static void SeedIfEmpty(AppDbContext db)
    {
        if (!db.Categories.Any())
        {
            SeedCatalog(db);
        }

        if (!db.UserAccounts.Any())
        {
            SeedUsersCapsulesAndReports(db);
        }

        EnsureAdminAccounts(db);
    }

    private static void EnsureAdminAccounts(AppDbContext db)
    {
        EnsureAdmin(
            db,
            "admin.one@memorylane.com",
            "Главный админ",
            "AdminOne123!");
        EnsureAdmin(
            db,
            "admin.two@memorylane.com",
            "Резервный админ",
            "AdminTwo123!");
        db.SaveChanges();
    }

    private static void EnsureAdmin(AppDbContext db, string email, string displayName, string password)
    {
        var existing = db.UserAccounts.FirstOrDefault(x => x.Email == email);
        if (existing == null)
        {
            db.UserAccounts.Add(new UserAccountData
            {
                Email = email,
                DisplayName = displayName,
                Role = "admin",
                Password = password,
                CreatedAtUtc = DateTime.UtcNow,
                NotifyEmailEnabled = true,
                NotifyPushEnabled = true,
                LoginAlertsEnabled = true
            });
            return;
        }

        existing.Role = "admin";
        existing.Password = password;
        existing.DisplayName = displayName;
    }

    private static void SeedCatalog(AppDbContext db)
    {
        var personal = new CategoryData { Name = "Личное" };
        var dreams = new CategoryData { Name = "Мечты" };
        var publicCat = new CategoryData { Name = "Публичное" };
        db.Categories.AddRange(personal, dreams, publicCat);
        db.SaveChanges();

        db.Products.AddRange(
            new ProductData
            {
                Name = "Послание потомкам",
                Price = 2999,
                CategoryId = personal.Id,
                Description = "Как мы жили в 2024 году",
                Image = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=500"
            },
            new ProductData
            {
                Name = "Письмо в 2030 год",
                Price = 1999,
                CategoryId = personal.Id,
                Description = "Мои цели на десятилетие",
                Image = "https://images.unsplash.com/photo-1484807352052-23338990c6c6?w=500"
            },
            new ProductData
            {
                Name = "Мечты о космосе",
                Price = 3999,
                CategoryId = dreams.Id,
                Description = "Записка о полете на Марс",
                Image = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=500"
            },
            new ProductData
            {
                Name = "Секретный рецепт",
                Price = 1499,
                CategoryId = personal.Id,
                Description = "Бабушкин пирог",
                Image = "https://images.unsplash.com/photo-1556910103-1c02745aae4d?w=500"
            },
            new ProductData
            {
                Name = "Капсула времени 2024",
                Price = 4999,
                CategoryId = publicCat.Id,
                Description = "События этого года",
                Image = "https://images.unsplash.com/photo-1461360228754-6e81c478b882?w=500"
            },
            new ProductData
            {
                Name = "Путешествие в будущее",
                Price = 2499,
                CategoryId = dreams.Id,
                Description = "Маршрут моей мечты",
                Image = "https://images.unsplash.com/photo-1488085061387-422e29b40080?w=500"
            });

        db.SaveChanges();
    }

    private static void SeedUsersCapsulesAndReports(AppDbContext db)
    {
        var u1 = new UserAccountData
        {
            Email = "demo@memorylane.com",
            DisplayName = "Демо пользователь",
            Role = "user",
            Password = "demo123",
            CreatedAtUtc = DateTime.UtcNow.AddDays(-30),
            NotifyEmailEnabled = true,
            NotifyPushEnabled = true,
            LoginAlertsEnabled = true
        };
        var u2 = new UserAccountData
        {
            Email = "maria@example.com",
            DisplayName = "Мария",
            Role = "moderator",
            Password = "maria123",
            CreatedAtUtc = DateTime.UtcNow.AddDays(-14),
            NotifyEmailEnabled = true,
            NotifyPushEnabled = false,
            LoginAlertsEnabled = true
        };
        db.UserAccounts.AddRange(u1, u2);
        db.SaveChanges();

        var now = DateTime.UtcNow;
        var pastOpen = now.AddDays(-2);
        var futureOpen = now.AddDays(30);

        var capPublicOpened = new TimeCapsuleData
        {
            OwnerUserId = u1.Id,
            ContentType = CapsuleContentType.Text,
            Title = "Добро пожаловать в ленту",
            TextContent = "Это публичная капсула, уже открытая для всех.",
            OpenAtUtc = pastOpen,
            CreatedAtUtc = now.AddDays(-10),
            RecipientEmail = "reader@example.com",
            IsPublic = true
        };

        var capSealed = new TimeCapsuleData
        {
            OwnerUserId = u1.Id,
            ContentType = CapsuleContentType.Text,
            Title = "Личное письмо будущему",
            TextContent = "Содержимое скрыто до даты открытия.",
            OpenAtUtc = futureOpen,
            CreatedAtUtc = now.AddDays(-5),
            RecipientEmail = "demo@memorylane.com",
            IsPublic = false
        };

        var capLink = new TimeCapsuleData
        {
            OwnerUserId = u2.Id,
            ContentType = CapsuleContentType.Link,
            Title = "Ссылка на воспоминание",
            LinkUrl = "https://memorylane.example.com/story/1",
            OpenAtUtc = pastOpen,
            CreatedAtUtc = now.AddDays(-3),
            RecipientEmail = u2.Email,
            IsPublic = true
        };

        db.TimeCapsules.AddRange(capPublicOpened, capSealed, capLink);
        db.SaveChanges();

        db.CapsuleLocations.Add(new CapsuleLocationData
        {
            CapsuleId = capPublicOpened.Id,
            Latitude = 48.8566,
            Longitude = 2.3522,
            PlaceLabel = "Париж — открой, когда будешь здесь"
        });

        db.ModerationReports.Add(new ModerationReportData
        {
            CapsuleId = capLink.Id,
            ReporterEmail = "moderator@memorylane.com",
            Reason = "Подозрение на спам в публичной ленте",
            Status = ReportStatus.Open,
            CreatedAtUtc = now.AddDays(-1)
        });

        db.SaveChanges();
    }
}
