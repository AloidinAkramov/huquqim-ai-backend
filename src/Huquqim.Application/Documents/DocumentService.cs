using System.Text.Json;
using System.Text.RegularExpressions;
using Huquqim.Application.Commons.Persistence;
using Huquqim.Application.Documents.Contracts;
using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Entities.Documents;
using Huquqim.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Huquqim.Application.Documents;

/// <summary>
/// Hujjat generatori (TZ Modul 3). Shablonni {{placeholder}}'lar bo'yicha
/// foydalanuvchi ma'lumotlari bilan to'ldiradi.
/// </summary>
public partial class DocumentService(
    IAppDbContext dbContext,
    Commons.Documents.IDocxGenerator docxGenerator) : IDocumentService
{
    public async Task<Result<List<TemplateResponse>>> GetTemplatesAsync(
        ECaseType? caseType = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.DocumentTemplates
            .Where(t => t.IsActive && !t.IsDeleted)
            .AsNoTracking();

        if (caseType.HasValue)
            query = query.Where(t => t.CaseType == caseType.Value);

        var templates = await query
            .OrderBy(t => t.Type)
            .ToListAsync(cancellationToken);

        return templates.Select(MapTemplate).ToList();
    }

    public async Task<Result<DocumentResponse>> GenerateAsync(
        long userId,
        long caseId,
        GenerateDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        var theCase = await dbContext.Cases
            .FirstOrDefaultAsync(c => c.Id == caseId && !c.IsDeleted, cancellationToken);

        if (theCase is null)
            return DocumentErrors.CaseNotFound;

        if (theCase.UserId != userId)
            return DocumentErrors.Forbidden;

        // Hujjat generatsiyasi faqat premium (pullik) tarif uchun.
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted, cancellationToken);
        if (user is null ||
            !SubscriptionLimits.CanGenerateDocuments(user.SubscriptionTier, user.SubscriptionExpiresAt, DateTime.UtcNow))
        {
            return DocumentErrors.PremiumRequired;
        }

        var template = await dbContext.DocumentTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.TemplateId && t.IsActive && !t.IsDeleted, cancellationToken);

        if (template is null)
            return DocumentErrors.TemplateNotFound;

        // Majburiy maydonlarni tekshirish
        var fields = ParseFields(template.Fields);
        foreach (var field in fields.Where(f => f.Required))
        {
            if (!request.Values.TryGetValue(field.Key, out var value) || string.IsNullOrWhiteSpace(value))
                return DocumentErrors.MissingField(field.Label);
        }

        var content = FillTemplate(template.Body, request.Values);

        var document = new Document
        {
            CaseId = caseId,
            Type = template.Type,
            Title = template.Name,
            Content = content,
            FilledData = JsonSerializer.Serialize(request.Values),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        dbContext.Documents.Add(document);

        // Ish holatini yangilash
        if (theCase.Status < ECaseStatus.DocumentReady)
        {
            theCase.Status = ECaseStatus.DocumentReady;
            theCase.UpdatedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapDocument(document);
    }

    public async Task<Result<List<DocumentResponse>>> GetByCaseAsync(
        long userId,
        long caseId,
        CancellationToken cancellationToken = default)
    {
        var theCase = await dbContext.Cases
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == caseId && !c.IsDeleted, cancellationToken);

        if (theCase is null)
            return DocumentErrors.CaseNotFound;

        if (theCase.UserId != userId)
            return DocumentErrors.Forbidden;

        var documents = await dbContext.Documents
            .Where(d => d.CaseId == caseId && !d.IsDeleted)
            .AsNoTracking()
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);

        return documents.Select(MapDocument).ToList();
    }

    public async Task<Result<DocumentResponse>> GetByIdAsync(
        long userId,
        long documentId,
        CancellationToken cancellationToken = default)
    {
        var document = await dbContext.Documents
            .AsNoTracking()
            .Include(d => d.Case)
            .FirstOrDefaultAsync(d => d.Id == documentId && !d.IsDeleted, cancellationToken);

        if (document is null)
            return DocumentErrors.NotFound;

        if (document.Case.UserId != userId)
            return DocumentErrors.Forbidden;

        return MapDocument(document);
    }

    public async Task<Result<DocxFile>> DownloadDocxAsync(
        long userId,
        long documentId,
        CancellationToken cancellationToken = default)
    {
        var document = await dbContext.Documents
            .AsNoTracking()
            .Include(d => d.Case)
            .FirstOrDefaultAsync(d => d.Id == documentId && !d.IsDeleted, cancellationToken);

        if (document is null)
            return DocumentErrors.NotFound;

        if (document.Case.UserId != userId)
            return DocumentErrors.Forbidden;

        var bytes = docxGenerator.Generate(document.Title, document.Content);
        var fileName = $"{Sanitize(document.Title)}.docx";

        return new DocxFile(fileName, bytes);
    }

    public async Task<Result<DocxFile>> FillTemplateDocxAsync(
        long userId,
        FillTemplateRequest request,
        CancellationToken cancellationToken = default)
    {
        // Premium tekshiruvi (hujjat yuklab olish pullik)
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted, cancellationToken);
        if (user is null ||
            !SubscriptionLimits.CanGenerateDocuments(user.SubscriptionTier, user.SubscriptionExpiresAt, DateTime.UtcNow))
        {
            return DocumentErrors.PremiumRequired;
        }

        var template = await dbContext.DocumentTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.TemplateId && t.IsActive && !t.IsDeleted, cancellationToken);

        if (template is null)
            return DocumentErrors.TemplateNotFound;

        // Majburiy maydonlarni tekshirish
        var fields = ParseFields(template.Fields);
        foreach (var field in fields.Where(f => f.Required))
        {
            if (!request.Values.TryGetValue(field.Key, out var value) || string.IsNullOrWhiteSpace(value))
                return DocumentErrors.MissingField(field.Label);
        }

        var content = FillTemplate(template.Body, request.Values);
        var bytes = docxGenerator.Generate(template.Name, content);
        var fileName = $"{Sanitize(template.Name)}.docx";

        return new DocxFile(fileName, bytes);
    }

    /// <summary>Fayl nomi uchun xavfsiz matn (faqat harf, raqam, bo'shliq).</summary>
    private static string Sanitize(string name)
    {
        var safe = new string(name.Where(c => char.IsLetterOrDigit(c) || c is ' ' or '-' or '_').ToArray()).Trim();
        return string.IsNullOrEmpty(safe) ? "hujjat" : safe;
    }

    /// <summary>{{key}} placeholder'larni qiymatlar bilan almashtiradi.</summary>
    private static string FillTemplate(string body, Dictionary<string, string> values)
    {
        return PlaceholderRegex().Replace(body, match =>
        {
            var key = match.Groups[1].Value.Trim();
            return values.TryGetValue(key, out var value) ? value : match.Value;
        });
    }

    private static List<TemplateField> ParseFields(string? fieldsJson)
    {
        if (string.IsNullOrWhiteSpace(fieldsJson))
            return new List<TemplateField>();

        try
        {
            return JsonSerializer.Deserialize<List<TemplateField>>(fieldsJson,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new List<TemplateField>();
        }
        catch
        {
            return new List<TemplateField>();
        }
    }

    private static TemplateResponse MapTemplate(DocumentTemplate t) => new()
    {
        Id = t.Id,
        Type = t.Type,
        CaseType = t.CaseType,
        Name = t.Name,
        Description = t.Description,
        Fields = ParseFields(t.Fields)
    };

    private static DocumentResponse MapDocument(Document d) => new()
    {
        Id = d.Id,
        CaseId = d.CaseId,
        Type = d.Type,
        Title = d.Title,
        Content = d.Content,
        CreatedAt = d.CreatedAt
    };

    [GeneratedRegex(@"\{\{\s*([^}]+?)\s*\}\}")]
    private static partial Regex PlaceholderRegex();
}
