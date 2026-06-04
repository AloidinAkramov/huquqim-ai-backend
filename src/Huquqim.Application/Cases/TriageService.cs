using System.Text.Json;
using Huquqim.Application.Cases.Contracts;
using Huquqim.Application.Commons.Ai;
using Huquqim.Application.Commons.Persistence;
using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Huquqim.Application.Cases;

/// <summary>
/// Triage: foydalanuvchi muammosini huquqiy toifalarga ajratadi.
/// Jiddiy (advokat majburiy) ishlarda yuristga yo'naltiradi.
/// </summary>
public class TriageService(
    IAppDbContext dbContext,
    IClaudeBroker aiBroker,
    ILogger<TriageService> logger) : ITriageService
{
    /// <summary>Advokat tavsiya qilinadigan jiddiy toifalar va chegara foizi.</summary>
    private const int SeriousThreshold = 70;
    private static readonly string[] SeriousCategories = { "Jinoiy" };

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<TriageResult>> AnalyzeAsync(
        long userId,
        long caseId,
        CancellationToken cancellationToken = default)
    {
        var theCase = await dbContext.Cases
            .FirstOrDefaultAsync(c => c.Id == caseId && !c.IsDeleted, cancellationToken);

        if (theCase is null)
            return CaseErrors.NotFound;

        if (theCase.UserId != userId)
            return CaseErrors.Forbidden;

        var problem = theCase.Description ?? theCase.Title;

        TriageResult result;
        try
        {
            result = await ClassifyAsync(problem, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Triage AI xatosi (caseId={CaseId})", caseId);
            return Error.Failure("Triage.Failed", "Toifalashda xatolik. Birozdan keyin urinib ko'ring.");
        }

        // Natijani ishga saqlash
        theCase.TriageJson = JsonSerializer.Serialize(result);
        theCase.RecommendLawyer = result.RecommendLawyer;

        // Eng yuqori toifani ish turiga moslash
        var top = result.Categories.MaxBy(c => c.Percent);
        if (top is not null)
            theCase.Type = MapCategoryToType(top.Name);

        theCase.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async Task<Result<TriageResult?>> GetAsync(
        long userId,
        long caseId,
        CancellationToken cancellationToken = default)
    {
        var theCase = await dbContext.Cases
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == caseId && !c.IsDeleted, cancellationToken);

        if (theCase is null)
            return CaseErrors.NotFound;

        if (theCase.UserId != userId)
            return CaseErrors.Forbidden;

        if (string.IsNullOrWhiteSpace(theCase.TriageJson))
            return Result.Success<TriageResult?>(null);

        var parsed = JsonSerializer.Deserialize<TriageResult>(theCase.TriageJson, JsonOptions);
        return Result.Success(parsed);
    }

    private async Task<TriageResult> ClassifyAsync(string problem, CancellationToken cancellationToken)
    {
        var systemPrompt = """
            Sen O'zbekiston huquqi bo'yicha toifalash (klassifikatsiya) tizimisan.
            Foydalanuvchi muammosini quyidagi toifalarga ajrat va har biriga ISHONCH
            FOIZINI ber (0-100, jami 100 bo'lishi shart emas, lekin eng mosini yuqori qil):
            - Fuqarolik (shartnoma, mulk, qarz, zarar, ijara, oila)
            - Jinoiy (o'g'rilik, firibgarlik, tan jarohati, jiddiy huquqbuzarlik)
            - Ma'muriy (jarima, protokol, yo'l qoidalari, kichik huquqbuzarlik)
            - Mehnat (ish haqi, ishdan bo'shatish, mehnat nizosi)
            - Iste'molchi (sifatsiz tovar/xizmat, do'kon nizosi)

            FAQAT quyidagi JSON formatida javob ber, boshqa hech narsa yozma:
            {
              "categories": [{"name":"Jinoiy","percent":75}, {"name":"Fuqarolik","percent":20}, ...],
              "recommendLawyer": true,
              "lawyerReason": "qisqa sabab",
              "summary": "1-2 jumla umumiy izoh"
            }

            QOIDA: Agar muammo JINOIY toifaga 70% yoki undan ko'p tegishli bo'lsa
            (o'g'rilik, firibgarlik, jiddiy jinoyat), "recommendLawyer": true qil va
            sababda advokat zarurligini ayt. Aks holda false.
            """;

        var messages = new List<ChatMessage>
        {
            new(EMessageRole.User, $"Muammo: {problem}")
        };

        var completion = await aiBroker.CompleteAsync(systemPrompt, messages, cancellationToken);

        var json = ExtractJson(completion.Content);
        var parsed = JsonSerializer.Deserialize<TriageResult>(json, JsonOptions)
                     ?? new TriageResult();

        // Xavfsizlik: server tomonida ham qoidani qo'llaymiz (AI yanglishsa).
        var jinoiy = parsed.Categories.FirstOrDefault(c =>
            SeriousCategories.Contains(c.Name, StringComparer.OrdinalIgnoreCase));

        var recommend = parsed.RecommendLawyer ||
                        (jinoiy is not null && jinoiy.Percent >= SeriousThreshold);

        return parsed with
        {
            RecommendLawyer = recommend,
            Categories = parsed.Categories.OrderByDescending(c => c.Percent).ToList()
        };
    }

    /// <summary>AI javobidan JSON qismini ajratib oladi (markdown bloklarni tozalaydi).</summary>
    private static string ExtractJson(string content)
    {
        var text = content.Trim();
        var start = text.IndexOf('{');
        var end = text.LastIndexOf('}');
        if (start >= 0 && end > start)
            return text.Substring(start, end - start + 1);
        return "{}";
    }

    private static ECaseType MapCategoryToType(string name) => name.ToLowerInvariant() switch
    {
        "iste'molchi" or "istemolchi" => ECaseType.Consumer,
        "mehnat" => ECaseType.Labor,
        "ma'muriy" or "mamuriy" => ECaseType.Administrative,
        "fuqarolik" => ECaseType.Civil,
        "jinoiy" => ECaseType.RequiresLawyer,
        _ => ECaseType.Unknown
    };
}
