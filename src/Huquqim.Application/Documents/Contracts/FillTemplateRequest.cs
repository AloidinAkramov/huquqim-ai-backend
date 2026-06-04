namespace Huquqim.Application.Documents.Contracts;

/// <summary>
/// Shablonni to'ldirib to'g'ridan docx olish so'rovi (ish saqlanmaydi).
/// </summary>
public record FillTemplateRequest
{
    public long TemplateId { get; set; }

    public Dictionary<string, string> Values { get; set; } = new();
}
