namespace Huquqim.Application.Documents.Contracts;

public record GenerateDocumentRequest
{
    /// <summary>Foydalanilayotgan shablon Id.</summary>
    public long TemplateId { get; set; }

    /// <summary>To'ldiriladigan maydon qiymatlari: key -> value.</summary>
    public Dictionary<string, string> Values { get; set; } = new();
}
