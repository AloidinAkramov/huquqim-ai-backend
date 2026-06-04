using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Enums;

namespace Huquqim.Domain.Entities.Conversations;

/// <summary>
/// Suhbatdagi bitta xabar (foydalanuvchi yoki AI).
/// </summary>
public class Message : AuditableModelBase<long>
{
    public long ConversationId { get; set; }

    public Conversation Conversation { get; set; } = default!;

    public EMessageRole Role { get; set; }

    public string Content { get; set; } = default!;

    /// <summary>
    /// RAG manbalari: AI javobi qaysi qonun moddalariga asoslangani (JSON).
    /// Masalan: ["FK 15-modda", "Iste'molchi qonuni 13-modda"].
    /// </summary>
    public string? Sources { get; set; }

    /// <summary>Claude API tomonidan ishlatilgan token soni (xarajat hisobi uchun).</summary>
    public int? TokenCount { get; set; }
}
