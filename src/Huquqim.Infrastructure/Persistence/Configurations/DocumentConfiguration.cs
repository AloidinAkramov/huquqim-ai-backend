using Huquqim.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Huquqim.Infrastructure.Persistence.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("documents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.Type).HasConversion<int>();
        builder.Property(x => x.FileFormat).HasConversion<int?>();
        builder.Property(x => x.FilledData).HasColumnType("jsonb");

        builder.HasIndex(x => x.CaseId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

public class DocumentTemplateConfiguration : IEntityTypeConfiguration<DocumentTemplate>
{
    public void Configure(EntityTypeBuilder<DocumentTemplate> builder)
    {
        builder.ToTable("document_templates");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Body).IsRequired();
        builder.Property(x => x.Type).HasConversion<int>();
        builder.Property(x => x.CaseType).HasConversion<int>();
        builder.Property(x => x.Fields).HasColumnType("jsonb");

        builder.HasIndex(x => new { x.CaseType, x.Type });

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
