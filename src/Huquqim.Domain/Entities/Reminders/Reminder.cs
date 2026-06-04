using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Entities.Cases;

namespace Huquqim.Domain.Entities.Reminders;

/// <summary>
/// Ish bo'yicha muddat eslatmasi (da'vo muddati, sud sanasi va h.k.).
/// </summary>
public class Reminder : AuditableModelBase<long>
{
    public long CaseId { get; set; }

    public Case Case { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string? Note { get; set; }

    /// <summary>Eslatma vaqti.</summary>
    public DateTime RemindAt { get; set; }

    public bool IsSent { get; set; }

    public bool IsCompleted { get; set; }
}
