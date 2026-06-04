using Huquqim.Domain.Enums;

namespace Huquqim.Application.Conversations.Contracts;

public record MessageResponse
{
    public long Id { get; init; }

    public EMessageRole Role { get; init; }

    public string Content { get; init; } = default!;

    /// <summary>Javob asoslangan qonun manbalari.</summary>
    public IReadOnlyList<string> Sources { get; init; } = new List<string>();

    public DateTime CreatedAt { get; init; }
}
