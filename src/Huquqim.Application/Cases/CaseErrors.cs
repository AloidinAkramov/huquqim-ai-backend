using Huquqim.Domain.Abstractions;

namespace Huquqim.Application.Cases;

public static class CaseErrors
{
    public static readonly Error NotFound =
        Error.NotFound("Case.NotFound", "Ish topilmadi.");

    public static readonly Error Forbidden =
        Error.Unauthorized("Case.Forbidden", "Bu ishga kirish huquqingiz yo'q.");
}
