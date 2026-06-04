using Huquqim.Domain.Abstractions;

namespace Huquqim.Application.Conversations;

public static class ConversationErrors
{
    public static readonly Error CaseNotFound =
        Error.NotFound("Conversation.CaseNotFound", "Ish topilmadi.");

    public static readonly Error NotFound =
        Error.NotFound("Conversation.NotFound", "Suhbat topilmadi.");

    public static readonly Error Forbidden =
        Error.Unauthorized("Conversation.Forbidden", "Bu suhbatga kirish huquqingiz yo'q.");

    public static readonly Error EmptyMessage =
        Error.Validation("Conversation.EmptyMessage", "Xabar bo'sh bo'lishi mumkin emas.");

    public static readonly Error AiUnavailable =
        Error.Failure("Conversation.AiUnavailable", "AI yordamchi vaqtincha ishlamayapti. Birozdan keyin urinib ko'ring.");

    public static readonly Error FreeLimitReached =
        Error.PaymentRequired(
            "Conversation.FreeLimitReached",
            "Bepul tarifda dastlabki tushuntirish berildi. To'liq, jiddiy javoblar va hujjatlar uchun premium tarifni tanlang.");
}
