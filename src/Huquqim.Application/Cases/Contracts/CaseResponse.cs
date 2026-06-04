using Huquqim.Domain.Enums;

namespace Huquqim.Application.Cases.Contracts;

public record CaseResponse
{
    public long Id { get; init; }

    public string Title { get; init; } = default!;

    public string? Description { get; init; }

    public ECaseType Type { get; init; }

    public ECaseStatus Status { get; init; }

    public string? ResponsibleAuthority { get; init; }

    public DateTime? Deadline { get; init; }

    public DateTime CreatedAt { get; init; }

    public int DocumentCount { get; init; }
}
