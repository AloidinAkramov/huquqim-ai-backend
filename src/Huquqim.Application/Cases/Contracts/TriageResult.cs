namespace Huquqim.Application.Cases.Contracts;

/// <summary>
/// Triage (toifalash) natijasi. AI muammoni huquqiy toifalarga ajratadi.
/// </summary>
public record TriageResult
{
    /// <summary>Toifalar va ularning foizlari (eng yuqoridan).</summary>
    public IReadOnlyList<TriageCategory> Categories { get; init; } = new List<TriageCategory>();

    /// <summary>Advokat (yurist) tavsiya qilinadimi — jiddiy/jinoiy ish bo'lsa.</summary>
    public bool RecommendLawyer { get; init; }

    /// <summary>Advokat tavsiyasi sababi (qisqa).</summary>
    public string? LawyerReason { get; init; }

    /// <summary>Qisqa umumiy izoh (foydalanuvchiga).</summary>
    public string? Summary { get; init; }
}

public record TriageCategory
{
    /// <summary>Toifa nomi: Fuqarolik, Jinoiy, Ma'muriy, Mehnat, Iste'molchi.</summary>
    public string Name { get; init; } = default!;

    /// <summary>Ishonch foizi (0-100).</summary>
    public int Percent { get; init; }
}
