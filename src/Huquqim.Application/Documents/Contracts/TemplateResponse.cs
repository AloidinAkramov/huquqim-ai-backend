using Huquqim.Domain.Enums;

namespace Huquqim.Application.Documents.Contracts;

public record TemplateResponse
{
    public long Id { get; init; }

    public EDocumentType Type { get; init; }

    public ECaseType CaseType { get; init; }

    public string Name { get; init; } = default!;

    public string? Description { get; init; }

    /// <summary>To'ldiriladigan maydonlar (key/label/required).</summary>
    public IReadOnlyList<TemplateField> Fields { get; init; } = new List<TemplateField>();
}

public record TemplateField
{
    public string Key { get; init; } = default!;

    public string Label { get; init; } = default!;

    public bool Required { get; init; }

    public string? Placeholder { get; init; }
}
