namespace Huquqim.Application.Conversations.Contracts;

public record SendMessageRequest
{
    /// <summary>Foydalanuvchi xabari.</summary>
    public string Content { get; set; } = default!;
}
