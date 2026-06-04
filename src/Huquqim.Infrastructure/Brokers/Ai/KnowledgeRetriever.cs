using Huquqim.Application.Commons.Ai;
using Huquqim.Domain.Entities.Knowledge;
using Huquqim.Domain.Enums;
using Huquqim.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Huquqim.Infrastructure.Brokers.Ai;

/// <summary>
/// MVP RAG retriever: bilim bazasidan kalit so'zlar bo'yicha mos qonun moddalarini topadi.
/// Keyingi bosqichda vektor qidiruv (Qdrant/pgvector) bilan almashtiriladi.
/// </summary>
public class KnowledgeRetriever(AppDbContext dbContext) : IKnowledgeRetriever
{
    // Qidiruvda e'tiborga olinmaydigan keng tarqalgan so'zlar.
    private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "va", "bilan", "uchun", "men", "meni", "mening", "bu", "shu", "ham",
        "edi", "bo'ldi", "qildi", "qanday", "nima", "kim", "qachon", "qayer"
    };

    public async Task<IReadOnlyList<LawArticle>> RetrieveAsync(
        string query,
        ECaseType? caseType = null,
        int limit = 5,
        CancellationToken cancellationToken = default)
    {
        var keywords = ExtractKeywords(query);
        if (keywords.Count == 0)
            return new List<LawArticle>();

        var baseQuery = dbContext.LawArticles
            .Where(a => !a.IsDeleted)
            .AsNoTracking();

        // Ish turini qonun toifasiga moslashtirish (yo'naltirilgan qidiruv).
        var category = MapCaseTypeToCategory(caseType);
        if (category.HasValue)
            baseQuery = baseQuery.Where(a => a.Category == category.Value);

        var candidates = await baseQuery.ToListAsync(cancellationToken);

        // Kalit so'z mosligi bo'yicha ball berib saralash.
        var ranked = candidates
            .Select(a => new
            {
                Article = a,
                Score = ScoreArticle(a, keywords)
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(limit)
            .Select(x => x.Article)
            .ToList();

        return ranked;
    }

    private static int ScoreArticle(LawArticle article, List<string> keywords)
    {
        var haystack = $"{article.Title} {article.Content}".ToLowerInvariant();
        return keywords.Count(k => haystack.Contains(k));
    }

    private static List<string> ExtractKeywords(string query)
    {
        return query
            .ToLowerInvariant()
            .Split(new[] { ' ', ',', '.', '!', '?', ';', ':', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length >= 3 && !StopWords.Contains(w))
            .Distinct()
            .ToList();
    }

    private static ELawCategory? MapCaseTypeToCategory(ECaseType? caseType) => caseType switch
    {
        ECaseType.Consumer => ELawCategory.ConsumerProtection,
        ECaseType.Labor => ELawCategory.LaborCode,
        ECaseType.Administrative => ELawCategory.AdministrativeCode,
        ECaseType.Civil or ECaseType.Rent => ELawCategory.CivilCode,
        _ => null
    };
}
