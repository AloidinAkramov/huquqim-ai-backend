namespace Huquqim.Application.Commons.Ai;

/// <summary>
/// AI dan qaytgan javob natijasi.
/// </summary>
public record AiCompletion
{
    public string Content { get; init; } = default!;

    /// <summary>Javobda foydalanilgan qonun manbalari (RAG).</summary>
    public IReadOnlyList<string> Sources { get; init; } = new List<string>();

    public int InputTokens { get; init; }

    public int OutputTokens { get; init; }

    public int TotalTokens => InputTokens + OutputTokens;
}
