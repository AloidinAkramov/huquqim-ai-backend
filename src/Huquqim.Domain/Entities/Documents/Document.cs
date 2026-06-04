using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Entities.Cases;
using Huquqim.Domain.Enums;

namespace Huquqim.Domain.Entities.Documents;

/// <summary>
/// Ish uchun generatsiya qilingan hujjat (ariza, pretenziya va h.k.).
/// </summary>
public class Document : AuditableModelBase<long>
{
    public long CaseId { get; set; }

    public Case Case { get; set; } = default!;

    public EDocumentType Type { get; set; }

    public string Title { get; set; } = default!;

    /// <summary>Hujjat matni (tahrirlanadigan, Markdown yoki HTML).</summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// Hujjatni to'ldirishda ishlatilgan foydalanuvchi maydonlari (JSON).
    /// Masalan: {"fio":"...", "sud_nomi":"...", "summa":"..."}.
    /// </summary>
    public string? FilledData { get; set; }

    /// <summary>Generatsiya qilingan fayl yo'li (docx/pdf), agar saqlangan bo'lsa.</summary>
    public string? FilePath { get; set; }

    public EDocumentFormat? FileFormat { get; set; }
}
