using Huquqim.Domain.Enums;

namespace Huquqim.Application.Documents.Contracts;

public record DocumentResponse
{
    public long Id { get; init; }

    public long CaseId { get; init; }

    public EDocumentType Type { get; init; }

    public string Title { get; init; } = default!;

    public string Content { get; init; } = default!;

    public DateTime CreatedAt { get; init; }
}
