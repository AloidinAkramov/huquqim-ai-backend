using Huquqim.Domain.Entities.Conversations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Huquqim.Infrastructure.Persistence.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("conversations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200);

        builder.HasIndex(x => x.CaseId);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Conversation)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role).HasConversion<int>();
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.Sources).HasColumnType("jsonb");

        builder.HasIndex(x => x.ConversationId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
