using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Enums;

namespace Huquqim.Application.Cases.Contracts;

public record CaseFilter : BaseFilter
{
    public ECaseType? Type { get; set; }

    public ECaseStatus? Status { get; set; }
}
