using Huquqim.Application.Commons.Persistence;
using Huquqim.Domain.Entities.Cases;
using Huquqim.Domain.Entities.Conversations;
using Huquqim.Domain.Entities.Documents;
using Huquqim.Domain.Entities.Knowledge;
using Huquqim.Domain.Entities.Reminders;
using Huquqim.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Huquqim.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Case> Cases { get; set; } = default!;
    public DbSet<Conversation> Conversations { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<Document> Documents { get; set; } = default!;
    public DbSet<DocumentTemplate> DocumentTemplates { get; set; } = default!;
    public DbSet<LawArticle> LawArticles { get; set; } = default!;
    public DbSet<Reminder> Reminders { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
