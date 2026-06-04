using Huquqim.Domain.Enums;

namespace Huquqim.Application.Cases.Contracts;

public record UpdateCaseRequest
{
    public string? Title { get; set; }

    public ECaseType? Type { get; set; }

    public ECaseStatus? Status { get; set; }

    public string? ResponsibleAuthority { get; set; }

    public DateTime? Deadline { get; set; }
}
