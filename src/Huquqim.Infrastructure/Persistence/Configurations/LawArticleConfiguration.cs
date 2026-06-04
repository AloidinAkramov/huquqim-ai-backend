using Huquqim.Domain.Entities.Knowledge;
using Huquqim.Domain.Entities.Reminders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Huquqim.Infrastructure.Persistence.Configurations;

public class LawArticleConfiguration : IEntityTypeConfiguration<LawArticle>
{
    public void Configure(EntityTypeBuilder<LawArticle> builder)
    {
        builder.ToTable("law_articles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Category).HasConversion<int>();
        builder.Property(x => x.SourceName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.ArticleNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Title).HasMaxLength(300);
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.SourceUrl).HasMaxLength(500);
        builder.Property(x => x.VectorId).HasMaxLength(100);

        builder.HasIndex(x => x.Category);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
{
    public void Configure(EntityTypeBuilder<Reminder> builder)
    {
        builder.ToTable("reminders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

        builder.HasIndex(x => x.CaseId);
        builder.HasIndex(x => x.RemindAt);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
