using System.Text;
using System.Text.Json;
using Huquqim.Application.Commons.Ai;
using Huquqim.Application.Commons.Persistence;
using Huquqim.Application.Conversations.Contracts;
using Huquqim.Domain.Abstractions;
using Huquqim.Domain.Entities.Conversations;
using Huquqim.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Huquqim.Application.Conversations;

/// <summary>
/// Chat + RAG yuragi. Foydalanuvchi xabarini qabul qiladi, bilim bazasidan
/// tegishli qonun moddalarini topadi, Claude'ga system prompt + kontekst bilan
/// yuboradi va javobni manbalari bilan saqlaydi.
/// </summary>
public class ConversationService(
    IAppDbContext dbContext,
    IClaudeBroker claudeBroker,
    IKnowledgeRetriever knowledgeRetriever,
    ILogger<ConversationService> logger) : IConversationService
{
    public async Task<Result<SendMessageResponse>> SendMessageAsync(
        long userId,
        long caseId,
        SendMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
            return ConversationErrors.EmptyMessage;

        var theCase = await dbContext.Cases
            .FirstOrDefaultAsync(c => c.Id == caseId && !c.IsDeleted, cancellationToken);

        if (theCase is null)
            return ConversationErrors.CaseNotFound;

        if (theCase.UserId != userId)
            return ConversationErrors.Forbidden;

        // Suhbatni topish yoki ochish
        var conversation = await dbContext.Conversations
            .Include(c => c.Messages.OrderBy(m => m.CreatedAt))
            .FirstOrDefaultAsync(c => c.CaseId == caseId && !c.IsDeleted, cancellationToken);

        // Tarif cheklovi: bepul foydalanuvchi dastlabki javoblardan keyin to'xtaydi.
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted, cancellationToken);
        if (user is not null &&
            !SubscriptionLimits.IsPremium(user.SubscriptionTier, user.SubscriptionExpiresAt, DateTime.UtcNow))
        {
            var aiReplies = conversation?.Messages.Count(m => m.Role == EMessageRole.Assistant) ?? 0;
            if (aiReplies >= SubscriptionLimits.FreeMessageLimit)
                return ConversationErrors.FreeLimitReached;
        }

        if (conversation is null)
        {
            conversation = new Conversation
            {
                CaseId = caseId,
                Title = theCase.Title,
                CreatedAt = DateTime.UtcNow
            };
            dbContext.Conversations.Add(conversation);
        }

        // Foydalanuvchi xabarini saqlash
        var userMessage = new Message
        {
            Conversation = conversation,
            Role = EMessageRole.User,
            Content = request.Content.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        conversation.Messages.Add(userMessage);

        // RAG: bilim bazasidan tegishli qonun moddalarini topish
        var articles = await knowledgeRetriever.RetrieveAsync(
            request.Content, theCase.Type, limit: 5, cancellationToken);

        var knowledgeContext = BuildKnowledgeContext(articles);
        var systemPrompt = SystemPrompts.BuildWithContext(knowledgeContext);

        // Suhbat tarixini AI formatiga o'tkazish
        var history = conversation.Messages
            .Where(m => m.Role != EMessageRole.System)
            .OrderBy(m => m.CreatedAt)
            .Select(m => new ChatMessage(m.Role, m.Content))
            .ToList();

        AiCompletion completion;
        try
        {
            completion = await claudeBroker.CompleteAsync(systemPrompt, history, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Claude broker xatosi (caseId={CaseId})", caseId);
            return ConversationErrors.AiUnavailable;
        }

        // Manbalar: retriever topgan moddalar + AI ko'rsatgan manbalar
        var sources = articles
            .Select(a => $"{a.SourceName} {a.ArticleNumber}")
            .Concat(completion.Sources)
            .Distinct()
            .ToList();

        var assistantMessage = new Message
        {
            Conversation = conversation,
            Role = EMessageRole.Assistant,
            Content = completion.Content,
            Sources = sources.Count > 0 ? JsonSerializer.Serialize(sources) : null,
            TokenCount = completion.TotalTokens,
            CreatedAt = DateTime.UtcNow
        };
        conversation.Messages.Add(assistantMessage);

        // Ish holatini yangilash: triage'dan tushuntirishga o'tkazish
        if (theCase.Status == ECaseStatus.Triage)
            theCase.Status = ECaseStatus.Explained;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new SendMessageResponse
        {
            ConversationId = conversation.Id,
            Reply = new MessageResponse
            {
                Id = assistantMessage.Id,
                Role = EMessageRole.Assistant,
                Content = assistantMessage.Content,
                Sources = sources,
                CreatedAt = assistantMessage.CreatedAt
            },
            Disclaimer = SystemPrompts.Disclaimer
        };
    }

    public async Task<Result<ConversationResponse>> GetByCaseAsync(
        long userId,
        long caseId,
        CancellationToken cancellationToken = default)
    {
        var theCase = await dbContext.Cases
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == caseId && !c.IsDeleted, cancellationToken);

        if (theCase is null)
            return ConversationErrors.CaseNotFound;

        if (theCase.UserId != userId)
            return ConversationErrors.Forbidden;

        var conversation = await dbContext.Conversations
            .AsNoTracking()
            .Include(c => c.Messages.Where(m => m.Role != EMessageRole.System).OrderBy(m => m.CreatedAt))
            .FirstOrDefaultAsync(c => c.CaseId == caseId && !c.IsDeleted, cancellationToken);

        if (conversation is null)
        {
            return new ConversationResponse
            {
                Id = 0,
                CaseId = caseId,
                Title = theCase.Title,
                CreatedAt = theCase.CreatedAt,
                Messages = new List<MessageResponse>()
            };
        }

        return new ConversationResponse
        {
            Id = conversation.Id,
            CaseId = caseId,
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt,
            Messages = conversation.Messages.Select(MapMessage).ToList()
        };
    }

    private static MessageResponse MapMessage(Message m) => new()
    {
        Id = m.Id,
        Role = m.Role,
        Content = m.Content,
        Sources = string.IsNullOrEmpty(m.Sources)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(m.Sources) ?? new List<string>(),
        CreatedAt = m.CreatedAt
    };

    private static string BuildKnowledgeContext(IReadOnlyList<Domain.Entities.Knowledge.LawArticle> articles)
    {
        if (articles.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        foreach (var a in articles)
        {
            sb.AppendLine($"## {a.SourceName} {a.ArticleNumber}" +
                          (string.IsNullOrWhiteSpace(a.Title) ? "" : $" — {a.Title}"));
            sb.AppendLine(a.Content);
            if (!string.IsNullOrWhiteSpace(a.SourceUrl))
                sb.AppendLine($"Manba: {a.SourceUrl}");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
