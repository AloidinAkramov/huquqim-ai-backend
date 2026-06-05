using System.Text.Json;
using Huquqim.Application.Commons.Security;
using Huquqim.Domain.Entities.Documents;
using Huquqim.Domain.Entities.Knowledge;
using Huquqim.Domain.Entities.Users;
using Huquqim.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Huquqim.Infrastructure.Persistence;

/// <summary>
/// Boshlang'ich ma'lumot: demo foydalanuvchi, namuna qonun moddalari va hujjat shablonlari.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(
        AppDbContext db,
        IPasswordHasher passwordHasher,
        string? demoEmail = null,
        string? demoPassword = null)
    {
        await SeedDemoUserAsync(db, passwordHasher, demoEmail, demoPassword);
        await SeedLawArticlesAsync(db);
        await SeedTemplatesAsync(db);
    }

    /// <summary>
    /// Demo foydalanuvchi — faqat email/parol konfiguratsiyada berilgan bo'lsa yaratiladi.
    /// Ma'lumotlar kodda emas, appsettings/env (DemoUser:Email, DemoUser:Password) orqali keladi.
    /// </summary>
    private static async Task SeedDemoUserAsync(
        AppDbContext db,
        IPasswordHasher passwordHasher,
        string? demoEmail,
        string? demoPassword)
    {
        if (string.IsNullOrWhiteSpace(demoEmail) || string.IsNullOrWhiteSpace(demoPassword))
            return;

        if (await db.Users.IgnoreQueryFilters().AnyAsync(u => u.Email == demoEmail))
            return;

        db.Users.Add(new User
        {
            FullName = "Demo Foydalanuvchi",
            Email = demoEmail,
            PasswordHash = passwordHasher.Hash(demoPassword),
            IsEmailConfirmed = true,
            SubscriptionTier = ESubscriptionTier.Monthly,
            SubscriptionExpiresAt = DateTime.UtcNow.AddYears(5),
            CreatedAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }

    private static async Task SeedLawArticlesAsync(AppDbContext db)
    {
        if (await db.LawArticles.IgnoreQueryFilters().AnyAsync())
            return;

        var now = DateTime.UtcNow;

        var articles = new List<LawArticle>
        {
            new()
            {
                Category = ELawCategory.ConsumerProtection,
                SourceName = "Iste'molchilar huquqlarini himoya qilish to'g'risidagi qonun",
                ArticleNumber = "4-modda",
                Title = "Iste'molchining asosiy huquqlari",
                Content = "Iste'molchi sifatli tovar (ish, xizmat), uning xavfsizligi, tovar haqida " +
                          "to'liq va ishonchli ma'lumot olish, shuningdek yetkazilgan zararning " +
                          "qoplanishi huquqiga ega.",
                SourceUrl = "https://lex.uz/docs/-32796",
                CreatedAt = now
            },
            new()
            {
                Category = ELawCategory.ConsumerProtection,
                SourceName = "Iste'molchilar huquqlarini himoya qilish to'g'risidagi qonun",
                ArticleNumber = "13-modda",
                Title = "Sifatsiz tovar uchun iste'molchining huquqlari",
                Content = "Sifatsiz tovar sotilgan iste'molchi o'z xohishiga ko'ra: tovarni almashtirish, " +
                          "narxni kamaytirish, kamchiliklarni bepul tuzatish yoki shartnomani bekor qilib, " +
                          "to'langan pulni qaytarib olishni talab qilishga haqli.",
                SourceUrl = "https://lex.uz/docs/-32796",
                CreatedAt = now
            },
            new()
            {
                Category = ELawCategory.CivilCode,
                SourceName = "Fuqarolik kodeksi",
                ArticleNumber = "15-modda",
                Title = "Zararlarning o'rni qoplanishi",
                Content = "Huquqi buzilgan shaxs, agar qonunda yoki shartnomada zararlarning o'rnini " +
                          "qoplash boshqacha hajmda nazarda tutilmagan bo'lsa, o'ziga yetkazilgan " +
                          "zararlarning o'rni to'liq qoplanishini talab qilishi mumkin.",
                SourceUrl = "https://lex.uz/docs/-111181",
                CreatedAt = now
            }
        };

        db.LawArticles.AddRange(articles);
        await db.SaveChangesAsync();
    }

    private static async Task SeedTemplatesAsync(AppDbContext db)
    {
        var existing = await db.DocumentTemplates
            .IgnoreQueryFilters()
            .Select(t => t.Name)
            .ToListAsync();
        var existingSet = existing.ToHashSet();

        var now = DateTime.UtcNow;
        var toAdd = new List<DocumentTemplate>();

        foreach (var def in TemplateSeedData.All)
        {
            // Idempotent: faqat yangi (nomi mavjud bo'lmagan) shablonlarni qo'shamiz.
            if (existingSet.Contains(def.Name))
                continue;

            toAdd.Add(new DocumentTemplate
            {
                Type = def.Type,
                CaseType = def.CaseType,
                Name = def.Name,
                Description = def.Description,
                Body = def.Body,
                Fields = JsonSerializer.Serialize(def.Fields.Select(f => new
                {
                    key = f.Key,
                    label = f.Label,
                    required = f.Required,
                    placeholder = f.Placeholder ?? ""
                })),
                IsActive = true,
                CreatedAt = now
            });
        }

        if (toAdd.Count == 0)
            return;

        db.DocumentTemplates.AddRange(toAdd);
        await db.SaveChangesAsync();
    }
}
