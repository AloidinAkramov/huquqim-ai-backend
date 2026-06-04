namespace Huquqim.Application.Conversations.Contracts;

public record SendMessageResponse
{
    public long ConversationId { get; init; }

    /// <summary>AI ning javob xabari.</summary>
    public MessageResponse Reply { get; init; } = default!;

    /// <summary>Har javobda ko'rsatiladigan disklaymer.</summary>
    public string Disclaimer { get; init; } = default!;
}
