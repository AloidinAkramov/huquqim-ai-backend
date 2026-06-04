using Huquqim.Domain.Abstractions;

namespace Huquqim.Application.Identity;

public static class IdentityErrors
{
    public static readonly Error EmailAlreadyExists =
        Error.Conflict("Identity.EmailAlreadyExists", "Bu email allaqachon ro'yxatdan o'tgan.");

    public static readonly Error InvalidCredentials =
        Error.Unauthorized("Identity.InvalidCredentials", "Email yoki parol noto'g'ri.");

    public static readonly Error UserNotFound =
        Error.NotFound("Identity.UserNotFound", "Foydalanuvchi topilmadi.");
}
