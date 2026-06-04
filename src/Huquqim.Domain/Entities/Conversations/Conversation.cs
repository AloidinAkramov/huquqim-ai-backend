using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Entities.Cases;

namespace Huquqim.Domain.Entities.Conversations;

/// <summary>
/// Bir ish doirasidagi AI bilan suhbat (chat sessiyasi).
/// </summary>
public class Conversation : AuditableModelBase<long>
{
    public long CaseId { get; set; }

    public Case Case { get; set; } = default!;

    public string? Title { get; set; }

    // Navigatsiya
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
