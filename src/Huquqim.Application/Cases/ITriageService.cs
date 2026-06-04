using Huquqim.Application.Cases.Contracts;
using Huquqim.Domain.Abstractions;

namespace Huquqim.Application.Cases;

public interface ITriageService
{
    /// <summary>
    /// Foydalanuvchi muammosini huquqiy toifalarga ajratadi va foiz beradi.
    /// Jiddiy (jinoiy va h.k.) ish bo'lsa advokat tavsiya qiladi.
    /// </summary>
    Task<Result<TriageResult>> AnalyzeAsync(long userId, long caseId, CancellationToken cancellationToken = default);

    /// <summary>Ish bo'yicha saqlangan triage natijasini qaytaradi (bo'lsa).</summary>
    Task<Result<TriageResult?>> GetAsync(long userId, long caseId, CancellationToken cancellationToken = default);
}
