using Huquqim.Domain.Entities.Knowledge;
using Huquqim.Domain.Enums;

namespace Huquqim.Application.Commons.Ai;

/// <summary>
/// RAG: foydalanuvchi savoliga mos qonun moddalarini bilim bazasidan topadi.
/// MVP'da text-search, keyinroq vektor qidiruv (Qdrant) ulanadi.
/// </summary>
public interface IKnowledgeRetriever
{
    Task<IReadOnlyList<LawArticle>> RetrieveAsync(
        string query,
        ECaseType? caseType = null,
        int limit = 5,
        CancellationToken cancellationToken = default);
}
