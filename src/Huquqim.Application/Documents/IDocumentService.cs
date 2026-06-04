using Huquqim.Application.Documents.Contracts;
using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Enums;

namespace Huquqim.Application.Documents;

public interface IDocumentService
{
    /// <summary>Mavjud hujjat shablonlarini qaytaradi (ish turi bo'yicha filtrlanishi mumkin).</summary>
    Task<Result<List<TemplateResponse>>> GetTemplatesAsync(ECaseType? caseType = null, CancellationToken cancellationToken = default);

    /// <summary>Shablonni foydalanuvchi ma'lumotlari bilan to'ldirib, hujjat yaratadi.</summary>
    Task<Result<DocumentResponse>> GenerateAsync(long userId, long caseId, GenerateDocumentRequest request, CancellationToken cancellationToken = default);

    /// <summary>Ish bo'yicha yaratilgan hujjatlar ro'yxati.</summary>
    Task<Result<List<DocumentResponse>>> GetByCaseAsync(long userId, long caseId, CancellationToken cancellationToken = default);

    /// <summary>Bitta hujjatni qaytaradi.</summary>
    Task<Result<DocumentResponse>> GetByIdAsync(long userId, long documentId, CancellationToken cancellationToken = default);

    /// <summary>Saqlangan hujjatni .docx (Word) fayl sifatida qaytaradi.</summary>
    Task<Result<DocxFile>> DownloadDocxAsync(long userId, long documentId, CancellationToken cancellationToken = default);

    /// <summary>Shablonni qiymatlar bilan to'ldirib, to'g'ridan-to'g'ri .docx qaytaradi (ish saqlanmaydi). Premium kerak.</summary>
    Task<Result<DocxFile>> FillTemplateDocxAsync(long userId, FillTemplateRequest request, CancellationToken cancellationToken = default);
}

/// <summary>Yuklab olish uchun docx fayl.</summary>
public record DocxFile(string FileName, byte[] Content);
