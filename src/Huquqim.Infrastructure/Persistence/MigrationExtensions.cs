using Huquqim.Application.Commons.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Huquqim.Infrastructure.Persistence;

public static class MigrationExtensions
{
    /// <summary>Ilova ishga tushganda migratsiyalarni qo'llaydi va boshlang'ich ma'lumotni joylaydi.</summary>
    public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        // Demo hisob ma'lumotlari env/appsettings'dan (kodda emas).
        var demoEmail = configuration["DemoUser:Email"];
        var demoPassword = configuration["DemoUser:Password"];

        await dbContext.Database.MigrateAsync();
        await DbSeeder.SeedAsync(dbContext, passwordHasher, demoEmail, demoPassword);
    }
}
