namespace Huquqim.Application.Conversations.Contracts;

public record ConversationResponse
{
    public long Id { get; init; }

    public long CaseId { get; init; }

    public string? Title { get; init; }

    public DateTime CreatedAt { get; init; }

    public IReadOnlyList<MessageResponse> Messages { get; init; } = new List<MessageResponse>();
}
