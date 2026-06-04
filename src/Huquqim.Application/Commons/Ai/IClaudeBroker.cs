namespace Huquqim.Application.Commons.Ai;

/// <summary>
/// Claude API (Anthropic) bilan ishlovchi broker.
/// </summary>
public interface IClaudeBroker
{
    /// <summary>
    /// Suhbat tarixini system prompt bilan birga Claude'ga yuboradi va javob oladi.
    /// </summary>
    /// <param name="systemPrompt">Tizim promti (RAG konteksti shu yerga qo'shiladi).</param>
    /// <param name="messages">Suhbat tarixi (foydalanuvchi/assistant).</param>
    Task<AiCompletion> CompleteAsync(
        string systemPrompt,
        IReadOnlyList<ChatMessage> messages,
        CancellationToken cancellationToken = default);
}
