using Huquqim.Application.Conversations.Contracts;
using Huquqim.Domain.Abstractions;

namespace Huquqim.Application.Conversations;

public interface IConversationService
{
    /// <summary>
    /// Ish bo'yicha xabar yuboradi. Suhbat bo'lmasa yangisini ochadi.
    /// RAG orqali bilim bazasidan javob oladi.
    /// </summary>
    Task<Result<SendMessageResponse>> SendMessageAsync(
        long userId,
        long caseId,
        SendMessageRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>Ish bo'yicha suhbatni (xabarlari bilan) qaytaradi.</summary>
    Task<Result<ConversationResponse>> GetByCaseAsync(
        long userId,
        long caseId,
        CancellationToken cancellationToken = default);
}
