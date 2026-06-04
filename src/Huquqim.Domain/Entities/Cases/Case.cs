using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Entities.Documents;
using Huquqim.Domain.Entities.Conversations;
using Huquqim.Domain.Entities.Reminders;
using Huquqim.Domain.Entities.Users;
using Huquqim.Domain.Enums;

namespace Huquqim.Domain.Entities.Cases;

/// <summary>
/// Foydalanuvchining huquqiy ishi (nizosi). Bir foydalanuvchida ko'p ish bo'lishi mumkin.
/// </summary>
public class Case : AuditableModelBase<long>
{
    public long UserId { get; set; }

    public User User { get; set; } = default!;

    /// <summary>Ishning qisqa sarlavhasi (foydalanuvchi muammosidan generatsiya qilinadi).</summary>
    public string Title { get; set; } = default!;

    /// <summary>Foydalanuvchi muammosining erkin tavsifi.</summary>
    public string? Description { get; set; }

    public ECaseType Type { get; set; } = ECaseType.Unknown;

    public ECaseStatus Status { get; set; } = ECaseStatus.Triage;

    /// <summary>Qaysi davlat organi yoki sud bilan ishlash kerak.</summary>
    public string? ResponsibleAuthority { get; set; }

    /// <summary>Da'vo muddati yoki boshqa muhim muddat (eslatma uchun).</summary>
    public DateTime? Deadline { get; set; }

    /// <summary>Triage (toifalash) natijasi JSON ko'rinishida — toifalar+foiz, advokat tavsiyasi.</summary>
    public string? TriageJson { get; set; }

    /// <summary>Jiddiy ish — advokat tavsiya qilinadimi (jinoiy va h.k.).</summary>
    public bool RecommendLawyer { get; set; }

    // Navigatsiya
    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    public ICollection<Document> Documents { get; set; } = new List<Document>();

    public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}
