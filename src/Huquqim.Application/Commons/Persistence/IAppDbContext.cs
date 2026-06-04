using Huquqim.Domain.Entities.Cases;
using Huquqim.Domain.Entities.Conversations;
using Huquqim.Domain.Entities.Documents;
using Huquqim.Domain.Entities.Knowledge;
using Huquqim.Domain.Entities.Reminders;
using Huquqim.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Huquqim.Application.Commons.Persistence;

/// <summary>
/// Application qatlami uchun DbContext abstraksiyasi.
/// Infrastructure'dagi AppDbContext shuni amalga oshiradi.
/// </summary>
public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Case> Cases { get; }
    DbSet<Conversation> Conversations { get; }
    DbSet<Message> Messages { get; }
    DbSet<Document> Documents { get; }
    DbSet<DocumentTemplate> DocumentTemplates { get; }
    DbSet<LawArticle> LawArticles { get; }
    DbSet<Reminder> Reminders { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
