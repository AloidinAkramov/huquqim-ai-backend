using Huquqim.Domain.Abstractions;

namespace Huquqim.Application.Documents;

public static class DocumentErrors
{
    public static readonly Error CaseNotFound =
        Error.NotFound("Document.CaseNotFound", "Ish topilmadi.");

    public static readonly Error TemplateNotFound =
        Error.NotFound("Document.TemplateNotFound", "Hujjat shabloni topilmadi.");

    public static readonly Error NotFound =
        Error.NotFound("Document.NotFound", "Hujjat topilmadi.");

    public static readonly Error Forbidden =
        Error.Unauthorized("Document.Forbidden", "Bu hujjatga kirish huquqingiz yo'q.");

    public static Error MissingField(string label) =>
        Error.Validation("Document.MissingField", $"\"{label}\" maydoni to'ldirilishi shart.");

    public static readonly Error PremiumRequired =
        Error.PaymentRequired(
            "Document.PremiumRequired",
            "Hujjat tayyorlash premium tarif uchun. To'liq xizmatdan foydalanish uchun tarifni tanlang.");
}
