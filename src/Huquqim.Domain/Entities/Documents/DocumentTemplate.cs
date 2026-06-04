using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Enums;

namespace Huquqim.Domain.Entities.Documents;

/// <summary>
/// Hujjat shabloni. Foydalanuvchi ma'lumotlari bilan to'ldiriladigan andoza.
/// </summary>
public class DocumentTemplate : AuditableModelBase<long>
{
    public EDocumentType Type { get; set; }

    public ECaseType CaseType { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    /// <summary>Shablon matni, {{maydon}} placeholder'lar bilan.</summary>
    public string Body { get; set; } = default!;

    /// <summary>
    /// To'ldiriladigan maydonlar ta'rifi (JSON).
    /// Masalan: [{"key":"fio","label":"F.I.O.","required":true}, ...].
    /// </summary>
    public string? Fields { get; set; }

    public bool IsActive { get; set; } = true;
}
