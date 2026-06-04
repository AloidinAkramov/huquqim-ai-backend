using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Enums;

namespace Huquqim.Domain.Entities.Knowledge;

/// <summary>
/// Bilim bazasidagi qonun moddasi (RAG manbasi). Lex.uz dan yig'iladi va vektor bazaga indekslanadi.
/// </summary>
public class LawArticle : AuditableModelBase<long>
{
    public ELawCategory Category { get; set; }

    /// <summary>Qonun/kodeks nomi. Masalan: "Fuqarolik kodeksi".</summary>
    public string SourceName { get; set; } = default!;

    /// <summary>Modda raqami. Masalan: "15-modda".</summary>
    public string ArticleNumber { get; set; } = default!;

    /// <summary>Modda sarlavhasi.</summary>
    public string? Title { get; set; }

    /// <summary>Modda to'liq matni (vektorlanadigan kontent).</summary>
    public string Content { get; set; } = default!;

    /// <summary>Lex.uz havolasi (manba sifatida ko'rsatiladi).</summary>
    public string? SourceUrl { get; set; }

    /// <summary>Vektor DB dagi nuqta identifikatori (Qdrant/Pinecone bilan bog'lash uchun).</summary>
    public string? VectorId { get; set; }

    public bool IsIndexed { get; set; }
}
